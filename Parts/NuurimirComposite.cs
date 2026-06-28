using System;
using System.Collections.Generic;

namespace XRL.World.Parts
{
    [Serializable]
    [HasGameBasedStaticCache]
    public class NuurimirComposite : IComposite
    {
        
        [NonSerialized]
        [GameBasedStaticCache(true, false, CreateInstance = false)]
        public static Dictionary<string, NuurimirComposite> _All;

        public string ID;

        public string OwnerID;

        public string Blueprint;

        public string Type;

        public GlobalLocation Location;
        
        public static Dictionary<string, NuurimirComposite> All
        {
            get
            {
                if (_All == null)
                {
                    _All = The.Game.GetObjectGameState("Nuurimir.Tracking") as Dictionary<string, NuurimirComposite>;
                    if (_All == null)
                    {
                        _All = new Dictionary<string, NuurimirComposite>();
                        The.Game.SetObjectGameState("Nuurimir.Tracking", _All);
                    }
                }
                return _All;
            }
        }
        
        public static List<NuurimirComposite> GetLocationFor(string Blueprint = null)
        {
            List<NuurimirComposite> list = new List<NuurimirComposite>();
            
            foreach (KeyValuePair<string, NuurimirComposite> item in All)
            {
                if ((!Blueprint.IsNullOrEmpty() && item.Value.Blueprint != Blueprint))
                {
                    continue;
                }
                
                list.Add(item.Value);
            }
            return list;
        }

        public static List<GameObject> ResolveRecordsFor(GameObject Owner = null, string Blueprint = null, string Type = null)
        {
            List<NuurimirComposite> recordsFor = GetLocationFor(Blueprint);
            List<GameObject> list = new List<GameObject>(recordsFor.Count);
            foreach (NuurimirComposite item in recordsFor)
            {
                if (The.ZoneManager.CachedObjects.TryGetValue(item.ID, out var value))
                {
                    list.Add(value);
                    continue;
                }
                Cell cell = item.Location.ResolveCell();
                if (cell == null)
                {
                    continue;
                }
                value = cell.FindObjectByID(item.ID) ?? cell.ParentZone.FindObjectByID(item.ID);
                if (value != null)
                {
                    if (value.IsTemporary)
                    {
                        All.Remove(item.ID);
                    }
                    else
                    {
                        list.Add(value);
                    }
                }
            }
            return list;
        }
    }
}