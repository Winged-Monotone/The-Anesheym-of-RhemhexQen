// XRL.World.Parts.GasPoison

using System;
using System.Collections.Generic;
using System.Threading;
using ConsoleLib.Console;
using UnityEngine;
using XRL.Core;
using XRL.Language;
using XRL.Rules;
using XRL.UI;

namespace XRL.World.Parts
{
    [Serializable]
    public class CrystniumSolidification : IPart
    {
        public CrystniumSolidification()
        {
        }

        public override bool WantEvent(int ID, int cascade)
        {
            if (!base.WantEvent(ID, cascade))
            {
                return ID == EnteredCellEvent.ID || ID == ObjectCreatedEvent.ID;
            }

            return true;
        }

        List<string> SoundFXs = new List<string>()
        {
            "{{M|shink!}}",
            "{{M|shnnnnk!}}",
            "{{M|sheeenk!}}",
        };

        public override bool HandleEvent(EnteredCellEvent E)
        {
            GameObject CrysGas = E.Object;
            Cell EnteredCell = E.Cell;
            if (EnteredCell.HasObjectWithBlueprint("CrystalReal1") ||
                EnteredCell.HasObjectWithBlueprint("CrystalReal2") ||
                EnteredCell.HasObjectWithBlueprint("CrystalReal3") ||
                EnteredCell.HasObjectWithBlueprint("CrystalReal4"))
            {
                List<Cell> AdjacentCells = CrysGas.CurrentCell.GetAdjacentCells(1);
                foreach (Cell cell in AdjacentCells)
                {
                    if (!cell.HasObjectWithBlueprint("CrystalReal1") || !cell.HasObjectWithBlueprint("cell") ||
                        !EnteredCell.HasObjectWithBlueprint("cell") ||
                        !EnteredCell.HasObjectWithBlueprint("CrystalReal4") &&
                        !EnteredCell.HasObjectWithBlueprint("AloeCrystna"))
                    {
                        if (Stat.Random(1, 100) <= 5)
                        {
                            if (Stat.Random(1, 100) >= 35)
                                cell.ParticleText(SoundFXs.GetRandomElement<string>(), 50.0f, 150);
                            cell.ClearAndAddObject("CrystalReal1");
                            CrysGas.ParticlePulse(".");
                        }
                        else
                        {
                        }
                    }
                }
            }

            return base.HandleEvent(E);
        }

        List<string> Parts = new List<string>()
        {
            "Combat",
            "Brain",
        };

        public override bool HandleEvent(ObjectCreatedEvent E)
        {
            if (E.Object != null)
            {
                GameObject ObjectInQ = E.Object;
                if (ObjectInQ.Blueprint != "CrystalReal1")
                {
                    return false;
                }

                if (ObjectInQ.Blueprint == "CrystalReal1")
                {
                    if (ObjectInQ.CurrentCell.HasObjectWithPart("Combat") &&
                        ObjectInQ.CurrentCell.HasObjectWithPart("Brain"))
                    {
                        List<GameObject> ToBeShunted = ObjectInQ.CurrentCell.GetObjectsWithPart("Combat");
                        var uDirect = Directions.GetRandomDirection();
                        foreach (GameObject obj in ToBeShunted)
                        {
                            obj.Move(
                                Direction: uDirect,
                                Forced: true,
                                System: false,
                                IgnoreGravity: false,
                                NoStack: true,
                                DoConfirmations: false,
                                NearestAvailable: true,
                                EnergyCost: 10);

                            if (obj.HasPart("Brain") && (obj.HasPart("Combat")))
                            {
                                int InitializeSaveFactor;
                                int InitializeFailFactor;
                                int uDamageValues = 1 + obj.baseHitpoints / 2 / obj.StatMod("Toughness", 1);
                                if (!obj.MakeSave(out InitializeSaveFactor, out InitializeFailFactor, "Agility", 21,
                                        null, null, null, false, false, false, false))
                                {
                                    obj.TakeDamage(Amount: ref uDamageValues,
                                        Attributes: "Slashing",
                                        DeathReason: "Impaled by a crystening shard.",
                                        Message: "from a crystening shard.",
                                        Environmental: true);
                                }
                            }
                        }
                    }
                }
            }

            return base.HandleEvent(E);
        }

        public override bool SameAs(IPart p)
        {
            return false;
        }
    }
}