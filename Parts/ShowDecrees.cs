using XRL.UI;

namespace XRL.World.Parts
{
    public class ShowDecrees : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                   || ID == GetInventoryActionsEvent.ID
                   || ID == InventoryActionEvent.ID;
        }
        
        public override bool HandleEvent(GetInventoryActionsEvent E)
        {

            E.AddAction("Read Decree", "read", "wmReadDecree", null, 'r');
            
            return base.HandleEvent(E);
        }
        
        public override bool HandleEvent(InventoryActionEvent E)
        {

            if (E.Command == "wmReadDecree")
            {
               Popup.Show(
                   "{{yellow|You gaze upon a great crystal-wrought slab, markings are etch as runes into its surface. You read: }}\n\n\n" +
                   "{{R|I.}} Hearken henceforth, no shah shall reign o'er kin with absolute sway. All pow'r shall from them be reft, and in its stead, they be held dear and honoured for their wisdom as the First-Mother. No decree may she overturn; should she attempt it, her people claim the right to cast her down." +
                   "\n\n{{O|II.}} All future decrees must be proclaimed in the presence of the people's Chosen, the Rhembearers, and the shah. No command shall be held in honour without this assembly." +
                   "\n\n{{W|III.}} The gates of Rhemhex'Qen shall remain shut to all strangers. We shall abide in these hallowed lands, forbidden from treading the wildwood. Whosoever cometh unto our gates shall be succoured with victuals, tools, and counsel. Yet none shall pass within." +
                   "\n\n{{G|IV.}} To venture forth into the outer realms doth brand thee as \"Denoth.\" Return unto these lands must be granted by the Council of Three. Without their solemn edicts, thou shalt be denied passage into Rhemhex'Qen." +
                   "\n\n{{C|V.}} Denounce all sultans, all sovereigns, all great lords of lofty renown, all mystics and the faithful. Deny the primal elders—the six, deny the Elder Cant, deny the song of thee who dwelled within THE DARKLING STAR. Deny, deny, deny. Naught but the endless dream endureth." +
                   "\n\n{{B|VI.}} Of reaching one’s second youth, they must embark on the sacred pilgrimage known as “Sunatalakum” to each grove of the heir-shahs. There they will be bestowed the “Sunatittum,” marking their birthright: entry into the Suttumkhofe." +
                   "\n\n{{M|VII.}} No creed, no edict, no rule shall encroach upon the natural right of liberties for mine own kin or those beyond it. May they name, ken, belong, and flourish as they list. May their culture bring solace, mirth, and diverseness to these lands. May we dwell in peace, renouncing the bigoted malice of those who came afore. We shall learn to dwell in concord.\n", "{{M|The Seven Edicts of Dae'Almophotea}}", LogMessage:false);
            }
            
            return base.HandleEvent(E);
        }
    }
}