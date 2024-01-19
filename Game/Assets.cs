// Ignore Spelling: Shockt

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game;
static class Assets {
    public static Texture2D PlayerTextureFront { get; private set; }
    public static Texture2D PlayerTextureBack { get; private set; }
    public static Texture2D PlayerTextureLeft { get; private set; }
    public static Texture2D PlayerTextureRight { get; private set; }
    public static Texture2D PlayerTextureRock { get; private set; }
    public static Texture2D PlayerTextureShockt { get; private set; }
    public static Texture2D PlayerTextureCake { get; private set; }

    public static Texture2D TreeTextureFront { get; private set; }
    public static Texture2D TreeTextureBack { get; private set; }
    public static Texture2D TreeTextureLeft { get; private set; }
    public static Texture2D TreeTextureRight { get; private set; }


    public static SpriteFont Font { get; private set; }

    public static void LoadContent(ContentManager content) {
        PlayerTextureFront = content.Load<Texture2D>("Assets/Chok_32");
        PlayerTextureBack = content.Load<Texture2D>("Assets/Chok_32_Back");
        PlayerTextureRight = content.Load<Texture2D>("Assets/Chok_32_Right");
        PlayerTextureLeft = content.Load<Texture2D>("Assets/Chok_32_Left");
        PlayerTextureRock = content.Load<Texture2D>("Assets/Chok_32_Rock");
        PlayerTextureShockt = content.Load<Texture2D>("Assets/Chok_32_Shockt");
        PlayerTextureCake = content.Load<Texture2D>("Assets/Chok_32_Cake");

        TreeTextureFront = content.Load<Texture2D>("Assets/Chok_32");
        TreeTextureBack = content.Load<Texture2D>("Assets/Chok_32");
        TreeTextureRight = content.Load<Texture2D>("Assets/Chok_32");
        TreeTextureLeft = content.Load<Texture2D>("Assets/Chok_32");

        // Font = content.Load<SpriteFont>("Font");
    }
}
