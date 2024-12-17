using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RayGuiCreator;
using ZeroElectric.Vinculum;

namespace Rogue
{
    internal class Options
    {
        public event EventHandler BackButtonPressedEvent;
        public void DrawMenu()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Raylib.BLACK);
            int elementX = 40;
            int elementY = 40;
            int menuWidth = Raylib.GetScreenWidth() * 4;
            MenuCreator creator = new MenuCreator(elementX, elementY, 20, menuWidth);
            creator.Label("Options");

            if (creator.LabelButton("Back"))
            {
                BackButtonPressedEvent.Invoke(this, EventArgs.Empty);

            }


            elementY += 30;
            if (creator.LabelButton("Quit"))
            {
            }
            Raylib.EndDrawing();
        }
    }
}
