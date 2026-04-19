using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoTiled.Extensions;

public static class Texture2DExtensions
{
    public static Color[,] GetColourData(this Texture2D texture, Rectangle sourceRectangle)
    {
        Color[] colourData1D = new Color[sourceRectangle.Width * sourceRectangle.Height];
        texture.GetData(0, rect: sourceRectangle, colourData1D, 0, colourData1D.Length);

        Color[,] colourData2D = new Color[sourceRectangle.Width, sourceRectangle.Height];
        for (int x = 0; x < sourceRectangle.Width; x++)
        {
            for (int y = 0; y < sourceRectangle.Height; y++)
            {
                // Get colour data from 1d array:
                // x + (y * sourceRectangle.Width) calculates the column offset needed to move down a row to get the array index for the pixel colour value of the next row in the texture
                // by skipping a fixed number of elements in the array
                // so if we start at 0,0 in the texture 0 + (0 * 64) = 0 so we get the array index 0 which has the pixel colour of 0,0 stored at index 0 in the array 
                // if we move to at 0,1 in the texture 0 + (1 * 64) = 64 so we get the the pixel colour of the texture at 0,1 stored at index 64 in the array - one row down
                // if we move to at 0,2 in the texture 0 + (2 * 64) = 64 so we get the the pixel colour of the texture at 0,2 stored at index 128 in the array 
                // Load colour data into 2d array:
                // we then load the colour data into a 2d array using the x and y values making it easier to retrieve the colour data later
                var arrayIndexOffset = x + (y * sourceRectangle.Width);
                colourData2D[x, y] = colourData1D[arrayIndexOffset];
            }
        }
        return colourData2D;
    }
 
}

