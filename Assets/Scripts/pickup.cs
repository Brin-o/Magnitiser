using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(PickupTrigger))]
public class pickup : MonoBehaviour
{

    public int id;
    public bool pickedUp = false;
    Vector3 originalScale;
    private void Start()
    {
        GameEvents.current.onCollectiblePickup += CoinPickedUp;
        originalScale = transform.localScale;
    }

    private void CoinPickedUp(int id)
    {
        if (!pickedUp && id == this.id)
        {
            pickedUp = true;
            GameEvents.current.pickups += 1;
            DOTween.Sequence()
                .Append(transform.DOPunchScale(Vector3.one * 0.05f, 0.05f, 1, 0))
                .Append(transform.DOScale(Vector3.zero, 0.2f))
                ;
            //Invoke("Disable", 0.25f);
        }
    }

    void Disable()
    {
        this.gameObject.SetActive(false);
    }

    public void CoinRestart()
    {
        pickedUp = false;
        DOTween.Sequence()
            .Append(transform.DOScale(originalScale, 0.05f))
            .Append(transform.DOPunchScale(Vector3.one * 0.3f, 0.2f, 4, 1));
    }
}
