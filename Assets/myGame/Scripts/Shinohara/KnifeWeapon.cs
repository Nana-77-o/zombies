using UnityEngine;

/// <summary>•Ší@ƒiƒCƒt </summary>
public class KnifeWeapon : WeaponBase
{
    [SerializeField, Header("UŒ‚‚Ì’†S")]
    Vector3 _attackCenter = default;
    [SerializeField, Header("UŒ‚”¼Œa")]
    float _attackRadius = 5f;
    [SerializeField, Header("UŒ‚ŠÔŠu")]
    float _attackInterval = 0.5f;

    float _attackTimer = 0f;

    private void OnEnable()
    {
        PlayerInput.SetEnterInput(InputType.Fire2, this.Attack);
    }
    private void OnDisable()
    {
        PlayerInput.LiftEnterInput(InputType.Fire2, this.Attack);
    }

    private void LateUpdate()
    {
        if (_attackInterval > _attackTimer)
        {
            _attackTimer += Time.deltaTime;
        }
    }

    public override void Attack()
    {
        if (_attackInterval > _attackTimer) { return; }
        var hitObjects = Physics.OverlapSphere(GetCenter(), _attackRadius);   //“–‚½‚Á‚½ƒIƒuƒWƒFƒNƒg‚ğ‘S‚Äæ“¾‚·‚é

        if (0 < hitObjects.Length)
        {
           
            //“Gˆê‘Ì‚Ì‚İ‚ÉUŒ‚‚·‚é‚½‚ß‚Ébreak‚µ‚Ä‚¢‚Ü‚·
            foreach (var obj in hitObjects)
            {
                if (obj.TryGetComponent(out IDamageApplicable target))
                {
                    target.AddDamage(GetDamage());
                    break;
                }
            }
        }

        _attackTimer = 0;
    }

    private void OnDrawGizmosSelected() //UŒ‚”ÍˆÍ‚ÌƒMƒYƒ‚‚ğ•\¦‚·‚é
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetCenter(), _attackRadius);
    }

    /// <summary>UŒ‚‚Ì’†S‚ğæ“¾‚·‚é</summary>
    /// <returns>UŒ‚‚Ì’†S</returns>
    Vector3 GetCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _attackCenter.z
           + this.transform.up * _attackCenter.y
           + this.transform.right * _attackCenter.x;
        return center;
    }
}
