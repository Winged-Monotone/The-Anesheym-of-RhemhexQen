// using System;
// using System.Collections.Generic;
// using System.Linq;
// using XRL.UI;
// using XRL.World.Effects;
//
// namespace XRL.World.Parts
// {
//     [Serializable]
//     public class NuurimirTracker : IPart
//     {
//         [NonSerialized]
//         private NuurimirTracker _Record;
//         
//         public int Flags = 16;
//         
//         public static Dictionary<string, VehicleRecord> All
//         {
//             get
//             {
//                 if (_All == null)
//                 {
//                     _All = The.Game.GetObjectGameState("Vehicle.Records") as Dictionary<string, VehicleRecord>;
//                     if (_All == null)
//                     {
//                         _All = new Dictionary<string, VehicleRecord>();
//                         The.Game.SetObjectGameState("Vehicle.Records", _All);
//                     }
//                 }
//                 return _All;
//             }
//         }
//         
//         public bool Tracker
//         {
//             get
//             {
//                 return Flags.HasBit(4);
//             }
//             set
//             {
//                 Flags.SetBit(4, value);
//             }
//         }
//         
//         public NuurimirTracker Record
//         {
//             get
//             {
//                 if (!Tracker)
//                 {
//                     return null;
//                 }
//                 if (_Record == null && !NuurimirTracker.All.TryGetValue(ParentObject.ID, out _Record) && !ParentObject.IsTemporary)
//                 {
//                     Dictionary<string, NuurimirTracker> all = NuurimirTracker.All;
//                     
//                     string iD = ParentObject.ID;
//                     
//                     NuurimirTracker obj = new NuurimirTracker
//                     {
//                         ID = ParentObject.ID,
//                         OwnerID = OwnerID,
//                         Blueprint = ParentObject.Blueprint,
//                         Location = (ParentObject.CurrentCell?.GetGlobalLocation() ?? new GlobalLocation()),
//                         Type = Type;
//                     };
//                     NuurimirTracker value = obj;
//                     _Record = obj;
//                     all[iD] = value;
//                 }
//                 return _Record;
//             }
//         }
//         
//         public override bool HandleEvent(EnteredCellEvent E)
//         {   
//             if (Tracker)
//             {
//                 Record?.Location.SetCell(E.Cell);
//             }
//             return base.HandleEvent(E);
//         }
//         
//         
//     }
// }