using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    #region Variaveis
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _moveSpeed;

    // Vari�veis para o Dash
    [SerializeField] private float _dashDistance; // Dist�ncia do dash
    [SerializeField] private float _dashDuration;   // Dura��o do dash
    [SerializeField] private float _dashCooldown; // Tempo de recarga do dash
    private bool _isDashing;
    private Vector3 _dashDirection;

    [SerializeField] private Button ButtonDash;
    #endregion

    private void Awake()
    {
        ButtonDash.onClick.AddListener(StartDash);
    }

    private void FixedUpdate()
    {
        if (!_isDashing)
        {
            //Aqui estamos definindo a velocidade do _rigidbody do Player
            _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joystick.Vertical * _moveSpeed);

            //Aqui estamos verificando se o joystick esta sendo movido 
            if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
            {
                //Aqui fazemos que o objeto olhe para dire��o que esta se movendo
                transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);

                _animator.SetBool("isRunning", true);
            }
            else
                _animator.SetBool("isRunning", false);

        }
    }

    private void StartDash()
    {
        if (!_isDashing)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        _isDashing = true;
        _animator.SetBool("isDashing", true); // Ativa o estado de dash no Animator
        _dashDirection = transform.forward;

        float dashTimer = 0f;
        while (dashTimer < _dashDuration)
        {
            _rigidbody.MovePosition(transform.position + _dashDirection * (_dashDistance / _dashDuration * Time.deltaTime));
            dashTimer += Time.deltaTime;
            yield return null;
        }

        _animator.SetBool("isDashing", false); // Desativa o estado de dash no Animator
        yield return new WaitForSeconds(_dashCooldown);
        _isDashing = false;
    }
}
