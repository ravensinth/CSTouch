using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClownSchool {
    public static class Settings {
        public static bool USE_SUBTRACTION { get; set; }
        public static bool USE_ADDITION { get; set; }
        public static bool USE_MULTIPLICATION { get; set; }

        public enum type { Subtraction, Addition, Multiplication }



        public static void ChangeSetting(type SetType) {
            switch (SetType) {
                case type.Subtraction:
                    USE_SUBTRACTION = !USE_SUBTRACTION;
                    break;
                case type.Addition:
                    USE_ADDITION = !USE_ADDITION;
                    break;
                case type.Multiplication:
                    USE_MULTIPLICATION = !USE_MULTIPLICATION;
                    break;
            }
        }
        public static List<type> GetAllChecked() {
            List<type> List = new List<type>();
            if (USE_SUBTRACTION) {
                List.Add(type.Subtraction);
            }
            if (USE_ADDITION) {
                List.Add(type.Addition);
            }
            if (USE_MULTIPLICATION) {
                List.Add(type.Multiplication);
            }
            return List;
        }

        public static bool GetValueByType(type SetType) {
            switch (SetType) {
                case type.Subtraction:
                    return USE_SUBTRACTION;
                case type.Addition:
                    return USE_ADDITION;
                case type.Multiplication:
                    return USE_MULTIPLICATION;
            }
            return false;
        }
    }
}
