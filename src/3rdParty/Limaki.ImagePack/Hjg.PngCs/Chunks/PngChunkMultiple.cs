﻿using System;

namespace Hjg.Pngcs.Chunks {
    /// <summary>
    /// A Chunk type that allows duplicate in an image
    /// </summary>
    public abstract class PngChunkMultiple : PngChunk {
        internal PngChunkMultiple(String id, ImageInfo imgInfo)
            : base(id, imgInfo) {

        }

        public sealed override bool AllowsMultiple() {
            return true;
        }

    }
}
