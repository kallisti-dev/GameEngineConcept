﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

using GameEngineConcept;
using GameEngineConcept.Graphics.Loaders;
using GameEngineConcept.Graphics;
using GameEngineConcept.Graphics.VertexBuffers;

namespace GameEngineTest.Tests
{
    //draws a sprite on screen
    public class SpriteDrawTest : BaseTester
    {
        DrawableDepthSet sprites;

        //see Triangle2DDrawTest for a general tutorial of the resource loading pattern
        public override void OnLoad(TestWindow window)
        {
            var tex = Texture.Allocate();
            tex.LoadImageFile("assets/sprite-example.png");

            var loader = new SpriteLoader(BufferUsageHint.DynamicDraw, VertexBuffer.Allocate());
            //TODO: test depth sorting
            loader.AddSprite(
                tex,
                new Vector2(0, 0),             //sprite position (top-left corner)
                new Rectangle(0, 0, 48, 21)    //source rectangle in sprite sheet
            );

            sprites = new DrawableDepthSet(loader.Load());
            
        }
        public  override void OnRenderFrame(FrameEventArgs e) 
        {
            sprites.Draw();
        }
    }
}