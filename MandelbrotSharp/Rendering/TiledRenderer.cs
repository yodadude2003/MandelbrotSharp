﻿/*
 *  Copyright 2018-2019 Chosen Few Software
 *  This file is part of MandelbrotSharp.
 *
 *  MandelbrotSharp is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  MandelbrotSharp is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with MandelbrotSharp.  If not, see <https://www.gnu.org/licenses/>.
 */
using MandelbrotSharp.Imaging;

namespace MandelbrotSharp.Rendering
{
    public class TiledRenderer : MandelbrotRenderer
    {
        public TiledRenderer(int width, int height) : base(width, height)
        {
            TotalCellsX = 1;
            TotalCellsY = 1;
        }

        protected int TotalCellsX { get; private set; }
        protected int TotalCellsY { get; private set; }

        protected int CellX { get; private set; }
        protected int CellY { get; private set; }

        protected int CellWidth => Width / TotalCellsX;
        protected int CellHeight => Height / TotalCellsY;

        protected virtual void UpdateCellCoords()
        {
            if (CellX < TotalCellsX - 1) { CellX++; }
            else if (CellY < TotalCellsY - 1) { CellX = 0; CellY++; }
            else { CellX = 0; CellY = 0; }
        }

        protected override void OnFrameFinished(FrameEventArgs e)
        {
            UpdateCellCoords();
            base.OnFrameFinished(e);
        }

        protected override IntPoint GetFrameFirstPixel()
        {
            return new IntPoint(CellX * CellWidth, CellY * CellHeight);
        }

        protected override IntPoint GetFrameLastPixel()
        {
            return new IntPoint((CellX + 1) * CellWidth, (CellY + 1) * CellHeight);
        }

        protected override void Configure(RenderSettings settings) {
            var renderSettings = (TiledRenderSettings)settings;
            TotalCellsX = renderSettings.TilesX;
            TotalCellsY = renderSettings.TilesY;
        }
    }
}
