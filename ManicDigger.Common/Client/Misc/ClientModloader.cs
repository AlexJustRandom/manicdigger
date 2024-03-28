using System;
namespace ManicDigger.Client.Misc
{
    public class ClientModloader
    {
        public ClientModloader()
        {
        }
        public void OnNewFrame(Game game, NewFrameEventArgs args) { }
        
        public void OnNewFrameFixed(Game game, NewFrameEventArgs args) { }
        
        public void OnBeforeNewFrameDraw2d(Game game, float deltaTime) { }
        
        public void OnNewFrameDraw2d(Game game, float deltaTime) { }
        
        public void OnBeforeNewFrameDraw3d(Game game, float deltaTime) { }
        
        public void OnNewFrameDraw3d(Game game, float deltaTime) { }

        public void OnKeyDown(Game game, KeyEventArgs args) { }
        public void OnKeyPress(Game game, KeyPressEventArgs args) { }
        public void OnKeyUp(Game game, KeyEventArgs args) { }
        public void OnMouseUp(Game game, MouseEventArgs args) { }
        public void OnMouseDown(Game game, MouseEventArgs args) { }
        public void OnMouseMove(Game game, MouseEventArgs args) { }
        public void OnMouseWheelChanged(Game game, MouseWheelEventArgs args) { }

        public void OnTouchStart(Game game, TouchEventArgs e) { }
        public void OnTouchMove(Game game, TouchEventArgs e) { }
        public void OnTouchEnd(Game game, TouchEventArgs e) { }
        public void OnUseEntity(Game game, OnUseEntityArgs e) { }
        public void OnHitEntity(Game game, OnUseEntityArgs e) { }

        public bool OnClientCommand(Game game, ClientCommandArgs args) { return false; }
        
    }
}
