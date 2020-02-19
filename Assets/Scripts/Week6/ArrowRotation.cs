using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    public float bobbingRate;
    private bool bobbing = false;

    private void Start()
    {
        transform.DOLocalRotate(new Vector3(0, 360.0f, 0), 2f, RotateMode.FastBeyond360)
            .SetRelative(true)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
        transform.DOLocalMove(Vector3.down * 2f, bobbingRate)
            .SetRelative(true)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
