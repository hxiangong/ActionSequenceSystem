﻿// ***************************************************************************
// Copyright (c) 2018 ZhongShan KPP Technology Co
// Copyright (c) 2018 Karsion
//   
// https://github.com/karsion
// Date: 2018-03-20 11:38
// ***************************************************************************

using System;
using UnityEngine;

namespace UnrealM
{
    //判定条件节点
    public class ActionNodeCondition : ActionNode
    {
        internal static readonly ObjectPool<ActionNodeCondition> opNodeCondition = new ObjectPool<ActionNodeCondition>(64);

        internal Func<bool> condition;
#if UNITY_EDITOR
        public static void GetObjectPoolInfo(out int countActive, out int countAll)
        {
            countActive = opNodeCondition.countActive;
            countAll = opNodeCondition.countAll;
        }
#endif

        internal static ActionNodeCondition Get(Func<bool> condition)
        {
            return opNodeCondition.Get().SetCondition(condition);
        }

        private ActionNodeCondition SetCondition(Func<bool> condition)
        {
            this.condition = condition;
            return this;
        }

        internal override bool Update(ActionSequence actionSequence, float deltaTime)
        {
            bool res = false;
            try
            {
                res = condition();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                actionSequence.Stop();
                return true;
            }

            return res;
        }

        internal override void Release()
        {
            condition = null;
            opNodeCondition.Release(this);
        }
    }
}