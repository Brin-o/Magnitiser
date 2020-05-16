using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(PickupTrigger))]
public class pickup : MonoBehaviour
{

    public int id;
    bool pickedUp = false;
    private void Start()
    {
        GameEvents.current.onCollectiblePickup += CoinPickedUp;
    }

    private void CoinPickedUp(int id)
    {
        if (!pickedUp && id == this.id)
        {
            GameEvents.current.pickups += 1;
            DOTween.Sequence()
                .Append(transform.DOPunchScale(Vector3.one * 0.05f, 0.05f, 1, 0))
                .Append(transform.DOScale(Vector3.zero, 0.2f))
                ;
            Destroy(gameObject, 0.25f);
        }
    }
}
