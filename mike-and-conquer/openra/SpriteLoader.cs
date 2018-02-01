using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;



namespace OpenRA.Graphics
{
    public interface ISpriteLoader
    {
        bool TryParseSprite(Stream s, out ISpriteFrame[] frames);
    }

    public interface ISpriteFrame
    {
        /// <summary>
        /// Size of the frame's `Data`.
        /// </summary>
        Size Size { get; }

        /// <summary>
        /// Size of the entire frame including the frame's `Size`.
        /// Think of this like a picture frame.
        /// </summary>
        Size FrameSize { get; }

        float2 Offset { get; }
        byte[] Data { get; }
        bool DisableExportPadding { get; }
    }



}