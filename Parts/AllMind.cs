// using System.Collections.Generic;
// using HistoryKit;
// using XRL.World;
//
// namespace XRl.World.Parts
// {
//     public class AllMind : IPart
//     {
//         public int AllMindSet = 0;
//         
//         public override bool WantEvent(int ID, int Cascade)
//         {
//             return base.WantEvent(ID, Cascade)
//                 || ID == ObjectCreatedEvent.ID;
//         }
//
//         public override bool HandleEvent(ObjectCreatedEvent E)
//         {
//             if (E.Object == ParentObject || AllMindSet == 0)
//             {
//                 AllMindSet = 1;
//                 // List<string> MentalMutations = new List<string>();
//
//                 var AllMentalMutations = XRL.World.;
//                 
//                 // MentalMutations.Add();
//
//             }
//             return base.HandleEvent(E);
//         }
//     }
// }