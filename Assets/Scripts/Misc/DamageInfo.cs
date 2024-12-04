using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Misc
{
    public enum DamageType
    {
        Physical,
        Energy,
        True,
        Elemental
    }
    public enum ElementalType
    {
        Fire,
        Water,
        Earth,
        Air
    }
    public class DamageInfo
    {
        public IEntity attacker { get; private set; }
        public float baseDamage { get; private set; } 
        public DamageType damageType { get; private set; }
        public ElementalType? elementalType { get; private set; }

        public DamageInfo(IEntity _attacker, float _baseDamage, DamageType _damageType, ElementalType? _elementalType = null)
        {
            attacker = _attacker;
            baseDamage = _baseDamage;
            damageType = _damageType;
            elementalType = _elementalType;

            if (damageType == DamageType.Elemental && elementalType != null)
            {
                elementalType = elementalType;
            }
        }


    }
}
