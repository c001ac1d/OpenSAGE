﻿using System.Collections.Generic;
using System.Numerics;
using OpenSage.Data.Ini;

namespace OpenSage.Logic.Object
{
    public sealed class RailroadBehavior : PhysicsBehavior
    {
        private uint _unknownObjectId;
        private uint _unknownInt1;
        private bool _unknownBool1;
        private bool _unknownBool2;
        private bool _unknownBool3;
        private bool _unknownBool4;
        private bool _unknownBool5;
        private bool _unknownBool6;
        private bool _unknownBool7;
        private bool _unknownBool8;
        private int _unknownInt2;
        private int _unknownInt3;
        private readonly RailroadBehaviorSomething _something1 = new();
        private readonly RailroadBehaviorSomething _something2 = new();

        internal RailroadBehavior(GameObject gameObject, GameContext context, PhysicsBehaviorModuleData moduleData)
            : base(gameObject, context, moduleData)
        {
        }

        internal override void Load(SaveFileReader reader)
        {
            reader.ReadVersion(2);

            base.Load(reader);

            reader.SkipUnknownBytes(4);

            reader.ReadObjectID(ref _unknownObjectId);

            _unknownInt1 = reader.ReadUInt32();

            reader.SkipUnknownBytes(4);

            reader.ReadBoolean(ref _unknownBool1);
            reader.ReadBoolean(ref _unknownBool2);
            reader.ReadBoolean(ref _unknownBool3);
            reader.ReadBoolean(ref _unknownBool4);
            reader.ReadBoolean(ref _unknownBool5);
            reader.ReadBoolean(ref _unknownBool6);
            reader.ReadBoolean(ref _unknownBool7);

            reader.SkipUnknownBytes(4);

            reader.ReadBoolean(ref _unknownBool8);
            reader.ReadInt32(ref _unknownInt2);
            reader.ReadInt32(ref _unknownInt3);

            _something1.Load(reader);

            _something2.Load(reader);
        }

        private sealed class RailroadBehaviorSomething
        {
            private float _unknownFloat1;
            private float _unknownFloat2;
            private float _unknownFloat3;
            private Vector3 _unknownVector;

            internal void Load(SaveFileReader reader)
            {
                reader.ReadVersion(1);

                reader.ReadSingle(ref _unknownFloat1);
                reader.ReadSingle(ref _unknownFloat2);
                reader.ReadSingle(ref _unknownFloat3);
                reader.ReadVector3(ref _unknownVector);

                var unknown12 = reader.ReadUInt32();
                if (unknown12 != 0xFACADE)
                {
                    throw new InvalidStateException();
                }

                var unknown13 = reader.ReadUInt32();
                if (unknown13 != 0xFACADE)
                {
                    throw new InvalidStateException();
                }

                var unknown14 = reader.ReadUInt32();
                if (unknown14 != 0xFACADE)
                {
                    throw new InvalidStateException();
                }
            }
        }
    }

    /// <summary>
    /// Requires object to follow waypoint path named with Tunnel, Disembark or Station with "Start"
    /// and "End" convention.
    /// </summary>
    public sealed class RailroadBehaviorModuleData : PhysicsBehaviorModuleData
    {
        internal new static RailroadBehaviorModuleData Parse(IniParser parser) => parser.ParseBlock(FieldParseTable);

        private new static readonly IniParseTable<RailroadBehaviorModuleData> FieldParseTable = PhysicsBehaviorModuleData.FieldParseTable.Concat(new IniParseTable<RailroadBehaviorModuleData>
        {
            { "PathPrefixName", (parser, x) => x.PathPrefixName = parser.ParseAssetReference() },

            { "RunningGarrisonSpeedMax", (parser, x) => x.RunningGarrisonSpeedMax = parser.ParseInteger() },
            { "KillSpeedMin", (parser, x) => x.KillSpeedMin = parser.ParseInteger() },
            { "Friction", (parser, x) => x.Friction = parser.ParseFloat() },
            { "BigMetalBounceSound", (parser, x) => x.BigMetalBounceSound = parser.ParseAssetReference() },
            { "SmallMetalBounceSound", (parser, x) => x.SmallMetalBounceSound = parser.ParseAssetReference() },
            { "MeatyBounceSound", (parser, x) => x.MeatyBounceSound = parser.ParseAssetReference() },
            { "ClicketyClackSound", (parser, x) => x.ClicketyClackSound = parser.ParseAssetReference() },
            { "WhistleSound", (parser, x) => x.WhistleSound = parser.ParseAssetReference() },

            { "IsLocomotive", (parser, x) => x.IsLocomotive = parser.ParseBoolean() },
            { "SpeedMax", (parser, x) => x.SpeedMax = parser.ParseFloat() },
            { "Acceleration", (parser, x) => x.Acceleration = parser.ParseFloat() },
            { "WaitAtStationTime", (parser, x) => x.WaitAtStationTime = parser.ParseInteger() },
            { "Braking", (parser, x) => x.Braking = parser.ParseFloat() },
            { "RunningSound", (parser, x) => x.RunningSound = parser.ParseAssetReference() },
            { "CrashFXTemplateName", (parser, x) => x.CrashFXTemplateName = parser.ParseAssetReference() },

            { "CarriageTemplateName", (parser, x) => x.CarriageTemplateNames.Add(parser.ParseAssetReference()) },
        });

        /// <summary>
        /// Waypoint prefix name.
        /// </summary>
        public string PathPrefixName { get; private set; }

        // Parameters for all carriages

        public int RunningGarrisonSpeedMax { get; private set; }
        public int KillSpeedMin { get; private set; }
        public float Friction { get; private set; }
        public string BigMetalBounceSound { get; private set; }
        public string SmallMetalBounceSound { get; private set; }
        public string MeatyBounceSound { get; private set; }
        public string ClicketyClackSound { get; private set; }
        public string WhistleSound { get; private set; }

        // Parameters that are only applicable when IsLocomotive = True.

        public bool IsLocomotive { get; private set; }
        public float SpeedMax { get; private set; }
        public float Acceleration { get; private set; }
        public int WaitAtStationTime { get; private set; }
        public float Braking { get; private set; }
        public string RunningSound { get; private set; }
        public string CrashFXTemplateName { get; private set; }

        public List<string> CarriageTemplateNames { get; } = new List<string>();

        internal override BehaviorModule CreateModule(GameObject gameObject, GameContext context)
        {
            return new RailroadBehavior(gameObject, context, this);
        }
    }
}
