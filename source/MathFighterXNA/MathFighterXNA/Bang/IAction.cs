using Microsoft.Xna.Framework;

namespace ClownSchool.Bang {
    public interface IAction {
        bool IsBlocking();
        bool IsComplete();

        void Block();
        void Unblock();
        void Update(GameTime gameTime);
        void Complete();        
    }
}
