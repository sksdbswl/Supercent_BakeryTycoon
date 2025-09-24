using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UnLockAnimation : MonoBehaviour
{
   private void OnEnable()
   {
      transform.localScale = Vector3.zero; 
      transform.DOScale(1f, 0.5f).SetEase(Ease.OutBounce);
   }
}
