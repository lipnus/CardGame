using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
public sealed class Card : MonoBehaviour
{
    [SerializeField] private CardView view;

    // 카드가 잡힐지 말지 제어할 대상
    [SerializeField] private XRGrabInteractable grabInteractable;

    private Rigidbody rb;
    private CardData data;

    public CardData Data => data;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // 인스펙터에 안 넣었으면 같은 오브젝트에서 자동 탐색
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();
    }

    // 카드 초기화: 데이터 주입 + 표현 갱신
    public void Initialize(CardData cardData)
    {
        data = cardData;
        if (view != null) view.Render(data);
        
        SetGrabbable(false);
        SetKinematic(true);

        grabInteractable.selectExited.AddListener(OnSelectExited);
    }
    
    private void OnSelectExited(SelectExitEventArgs args)
    {
        SetKinematic(false);
    }

    // 유저가 잡을 수 있는지 제어 (최상단만 true)
    public void SetGrabbable(bool isGrabbable)
    {
        if (grabInteractable != null)
            grabInteractable.enabled = isGrabbable;
    }

    // 덱에 쌓인 상태: 물리 안정화를 위해 잠깐 고정 같은 처리 가능
    public void SetKinematic(bool isKinematic)
    {
        if (rb) rb.isKinematic = isKinematic;
    }

 
}