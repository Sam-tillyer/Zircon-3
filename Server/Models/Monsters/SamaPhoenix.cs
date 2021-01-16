﻿using Library;
using Server.Envir;
using S = Library.Network.ServerPackets;


namespace Server.Models.Monsters
{
    public class SamaPhoenix : MonsterObject
    {
        public int RangeChance = 5;

        protected override bool InAttackRange()
        {
            if (Target.CurrentMap != CurrentMap) return false;

            return Target.CurrentLocation != CurrentLocation && Functions.InRange(CurrentLocation, Target.CurrentLocation, 2);
        }


        public override void ProcessTarget()
        {
            if (Target == null) return;

            if (!InAttackRange())
            {
                if (CanAttack)
                {
                    if (SEnvir.Random.Next(RangeChance) == 0)
                        RangeAttack();
                }


                if (CurrentLocation == Target.CurrentLocation)
                {
                    MirDirection direction = (MirDirection)SEnvir.Random.Next(8);
                    int rotation = SEnvir.Random.Next(2) == 0 ? 1 : -1;

                    for (int d = 0; d < 8; d++)
                    {
                        if (Walk(direction)) break;

                        direction = Functions.ShiftDirection(direction, rotation);
                    }
                }
                else
                    MoveTo(Target.CurrentLocation);
            }

            if (!CanAttack) return;

            if (SEnvir.Random.Next(RangeChance) > 0)
            {
                if (InAttackRange())
                    Attack();
            }
            else RangeAttack();
        }

        public virtual void RangeAttack()
        {
            switch (SEnvir.Random.Next(5))
            {
                case 0:
                    AttackMagic(MagicType.AdamantineFireBall, Element.Fire, true);
                    break;
                case 1:
                    LineAoE(10, -2, 2, MagicType.ScortchedEarth, Element.Fire);
                    break;
                case 2:
                    AttackAoE(2, MagicType.FireStorm, Element.Fire);
                    break;
                case 3:
                    AttackAoE(3, MagicType.SamaGuardianFire, Element.Fire);
                    break;
                case 4:
                    AttackAoE(3, MagicType.SamaPhoenixFire, Element.Fire, GetDC() * 2);
                    break;
            }
        }
    }
}
