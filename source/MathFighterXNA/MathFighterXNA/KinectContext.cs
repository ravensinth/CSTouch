using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.Interaction;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;

namespace ClownSchool {

    public class KinectContext {

        public KinectSensor Sensor { get; private set; }
        
        public Texture2D CurrentBitmap { get; private set; }      
        public List<Skeleton> Skeletons { get; private set; }
        public Dictionary<int, UserInfo> UserInfos { get; private set; }


        private GraphicsDevice graphicsDevice { get; set; }
        private InteractionStream interactionStream { get; set; }

        public KinectContext(GraphicsDevice device) {
            graphicsDevice = device;
            Skeletons = new List<Skeleton>();
            UserInfos = new Dictionary<int, UserInfo>();
        }

        public void Initialize() {
            foreach (var potentialSensor in KinectSensor.KinectSensors) {
                if (potentialSensor.Status == KinectStatus.Connected) {
                    this.Sensor = potentialSensor;
                    break;
                }
            }

            if (this.Sensor != null) {
                var parameters = new TransformSmoothParameters {
                    Smoothing = 0.0f,
                    Correction = 0.0f,
                    Prediction = 0.0f,
                    JitterRadius = 0.0f,
                    MaxDeviationRadius = 0.0f
                };

                interactionStream = new InteractionStream(Sensor, new InteractionClient());                 

                this.Sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
                this.Sensor.SkeletonStream.Enable(parameters);
                this.Sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);                

                try {
                    this.Sensor.Start();
                } catch (System.IO.IOException ex) {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                    this.Sensor = null;
                }
            }

            if (this.Sensor == null) {
                Debug.WriteLine("No kinect connected!");
            }

            this.CurrentBitmap = Assets.SplashLogo;
        }

        void ProcessDepthFrame() {
            using (var dif = this.Sensor.DepthStream.OpenNextFrame(0)) {
                if (dif != null) {
                    DepthImagePixel[] data = new DepthImagePixel[dif.PixelDataLength];
                    dif.CopyDepthImagePixelDataTo(data);

                    interactionStream.ProcessDepth(data, dif.Timestamp);
                }

            }
        }

        void ProcessColorFrame() {
            using (var cif = this.Sensor.ColorStream.OpenNextFrame(0)) {
                if (cif != null ) {
                    Byte[] pixelData = new Byte[cif.PixelDataLength];
                    cif.CopyPixelDataTo(pixelData);
                    
                    Byte[] bgraPixelData = new Byte[cif.PixelDataLength];
                    for (int i = 0; i < pixelData.Length; i += 4) {
                        bgraPixelData[i] = pixelData[i + 2];
                        bgraPixelData[i + 1] = pixelData[i + 1];
                        bgraPixelData[i + 2] = pixelData[i];
                        bgraPixelData[i + 3] = (Byte)255;
                    }
                    CurrentBitmap = new Texture2D(this.graphicsDevice, cif.Width, cif.Height); 
                    CurrentBitmap.SetData(bgraPixelData);
                }
            }
        }

        void ProcessSkeletonFrame() {
            using (var skeletonFrame = this.Sensor.SkeletonStream.OpenNextFrame(0)) {
                if (skeletonFrame != null) {
                    Skeleton[] skeletons = new Skeleton[skeletonFrame.SkeletonArrayLength];
                    skeletonFrame.CopySkeletonDataTo(skeletons);

                    if (Configuration.GRABBING_ENABLED) {
                        var accelerometerReading = this.Sensor.AccelerometerGetCurrentReading();
                        interactionStream.ProcessSkeleton(skeletons, accelerometerReading, skeletonFrame.Timestamp);
                    }

                    Skeletons.Clear();
                    foreach (Skeleton skel in skeletons) {
                        if (skel.TrackingState == SkeletonTrackingState.Tracked) {
                            Skeletons.Add(skel);             
                        }
                    }
                }
            }
        }

        void ProcessInteractionFrame() {
            using(var interactionFrame = interactionStream.OpenNextFrame(0)) {
                if (interactionFrame != null) {                   
                    UserInfo[] userInfo = new UserInfo[InteractionFrame.UserInfoArrayLength];
                    interactionFrame.CopyInteractionDataTo(userInfo);

                    UserInfos.Clear();
                    foreach (var skel in Skeletons) {
                        var ui = (from UserInfo u in userInfo where u.SkeletonTrackingId == skel.TrackingId select u).FirstOrDefault();
                        if (ui != null)
                            UserInfos.Add(skel.TrackingId, ui);
                    }
                }

            }
        }

        public Point SkeletonPointToScreen(SkeletonPoint skelpoint) {            
            ColorImagePoint colorPoint = Sensor.CoordinateMapper.MapSkeletonPointToColorPoint(skelpoint, ColorImageFormat.RgbResolution640x480Fps30);
            return new Point((int)(colorPoint.X / MainGame.KinectScaleX) + MainGame.KinectOffsetX, (int)(colorPoint.Y / MainGame.KinectScaleY) + MainGame.KinectOffsetY);
        }

        public Skeleton GetSkeletonById(int id) {
            return (from Skeleton s in Skeletons where s.TrackingId == id select s).FirstOrDefault();
        }

        public Skeleton GetFirstSkeleton() {
            return Skeletons.FirstOrDefault<Skeleton>();
        }

        public Skeleton GetLeftSkeleton() { 
            return (from Skeleton s in Skeletons orderby s.Position.X ascending select s).FirstOrDefault();
        }

        public Skeleton GetRightSkeleton() {            
            return (from Skeleton s in Skeletons where s != GetLeftSkeleton() orderby s.Position.X descending select s).FirstOrDefault();            
        }

        public void Update() {
            if (this.Sensor != null) {
                ProcessColorFrame();
                ProcessSkeletonFrame();

                if (Configuration.GRABBING_ENABLED) {
                    ProcessDepthFrame();
                    ProcessInteractionFrame();
                }
            }
        }

        public void StopSensor() {
            if (this.Sensor != null) {
                this.Sensor.Stop();
            }
        }
    }
}