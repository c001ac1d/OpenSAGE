﻿using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OpenSage.Data.Rep
{
    public sealed class ReplayFile
    {
        public ReplayHeader Header { get; private set; }
        public IReadOnlyList<ReplayChunk> Chunks { get; private set; }

        public static ReplayFile FromFileSystemEntry(FileSystemEntry entry)
        {
            using (var stream = entry.Open())
            using (var reader = new BinaryReader(stream, Encoding.Unicode, true))
            {
                var result = new ReplayFile
                {
                    Header = ReplayHeader.Parse(reader)
                };

                var chunks = new List<ReplayChunk>();
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    chunks.Add(ReplayChunk.Parse(reader));
                }
                result.Chunks = chunks;

                if (result.Header.NumTimecodes != chunks[chunks.Count - 1].Header.Timecode)
                {
                    throw new InvalidDataException();
                }

                return result;
            }
        }
    }
}
