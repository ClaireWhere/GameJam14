using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJam14.Game.Data;
static class SpriteData {
    public static Sprite PlayerSprite {
        get {
            TextureBundle textures = new TextureBundle(
                textures: new Dictionary<TextureType, Texture2D> {
                    { TextureType.FaceFront, Assets.PlayerTextureFront },
                    { TextureType.FaceLeft, Assets.PlayerTextureLeft },
                    { TextureType.FaceRight, Assets.PlayerTextureRight },
                    { TextureType.FaceBack, Assets.PlayerTextureBack },
                    { TextureType.ExpressionShock, Assets.PlayerTextureShockt },
                    { TextureType.ExpressionRizz, Assets.PlayerTextureRock },
                },
                initialState: TextureType.FaceFront
            );
            return new Sprite(
                textures: textures,
                position: Vector2.Zero,
                rotation: 0,
                scale: 1,
                sourceRectangle: new Rectangle(0, 0, Assets.PlayerTextureFront.Width, Assets.PlayerTextureFront.Height),
                color: Color.White,
                effects: SpriteEffects.None,
                layerDepth: 0
            );
        }
    }

    public static Sprite TreeSprite {
        get {
            TextureBundle textures = new TextureBundle(
                 textures: new Dictionary<TextureType, Texture2D> {
                    { TextureType.FaceFront, Assets.TreeTextureFront },
                    { TextureType.FaceLeft, Assets.TreeTextureLeft },
                    { TextureType.FaceRight, Assets.TreeTextureRight },
                    { TextureType.FaceBack, Assets.TreeTextureFront },
                },
                initialState: TextureType.FaceFront
            );
            return new Sprite(
                textures: textures,
                position: Vector2.Zero,
                rotation: 0,
                scale: 1,
                sourceRectangle: new Rectangle(0, 0, Assets.TreeTextureFront.Width, Assets.TreeTextureFront.Height),
                color: Color.White,
                effects: SpriteEffects.None,
                layerDepth: 0
            );
        }
    }
}
