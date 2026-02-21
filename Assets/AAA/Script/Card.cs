using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class Card : MonoBehaviour
{
    [SerializeField] private CardView view;
    [SerializeField] private MonoBehaviour grabbableToggle; // IXRGrabbableToggle 구현체 연결

    private Rigidbody rb;
    private CardData data;

    public CardData Data => data;

    private IXRGrabbableToggle GrabToggle => grabbableToggle as IXRGrabbableToggle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // 카드 초기화: 데이터 주입 + 표현 갱신
    public void Initialize(CardData cardData)
    {
        data = cardData;
        if (view != null) view.Render(data);
    }

    // 유저가 잡을 수 있는지 제어
    public void SetGrabbable(bool enabled)
    {
        if (GrabToggle != null)
            GrabToggle.SetGrabEnabled(enabled);
    }

    // 덱에 쌓인 상태: 물리 안정화를 위해 잠깐 고정 같은 처리 가능
    public void SetKinematic(bool isKinematic)
    {
        if (rb != null) rb.isKinematic = isKinematic;
    }
}