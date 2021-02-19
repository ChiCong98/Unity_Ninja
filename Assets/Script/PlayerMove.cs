using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D m_RiRgidbody;

    [SerializeField] private float m_Speed;

    private bool m_FacingRight;

    private bool m_Attack;

    private bool m_Slide;

    private Animator anim;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    private bool isGrounded;

    private bool jump;

    private bool jumpAttack;

    [SerializeField]
    private float jumpForce;
    // Start is called before the first frame update
    void Start()
    {
        m_FacingRight = true;
        m_RiRgidbody = GetComponent<Rigidbody2D>();
        m_Speed = 400f;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        FixedUpdate();
        HandleAttack();
        HandleJump();
        HandleLayers();
        ResetValue();
    }
    private void FixedUpdate()
    {
        if(m_RiRgidbody.velocity.y<0)
        {
            anim.SetBool("IsLand", true);
        }
        if (!this.anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            isGrounded = IsGrounded();
            m_RiRgidbody.velocity = new Vector2(horizontal * Time.deltaTime * m_Speed, m_RiRgidbody.velocity.y);
            Flip(horizontal);
            anim.SetFloat("Speed", Mathf.Abs(horizontal));
        }
        HandleSlie();
    }
    private void HandleAttack()
    {
        if(m_Attack&&!this.anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            anim.SetTrigger("attack");
            m_RiRgidbody.velocity = Vector2.zero;
        }
        if(jumpAttack && !isGrounded && !this.anim.GetCurrentAnimatorStateInfo(1).IsName("Ninja_JumpAttack"))
        {
            anim.SetBool("IsJumpAttack", true);
        }
        if(!jumpAttack && this.anim.GetCurrentAnimatorStateInfo(1).IsName("Ninja_JumpAttack"))
        {
            anim.SetBool("IsJumpAttack", false);
        }
    }
    private void HandleSlie()
    {
        if (m_Slide && !this.anim.GetCurrentAnimatorStateInfo(0).IsName("Ninja_Slide"))
        {
            anim.SetBool("IsSlide",true);
        }
        else if(!m_Slide &&this.anim.GetCurrentAnimatorStateInfo(0).IsName("Ninja_Slide"))
        {
            anim.SetBool("IsSlide", false);
        }
    }
    private void HandleJump()
    {
        if(isGrounded && jump)
        {
            isGrounded = false;
            m_RiRgidbody.AddForce(new Vector2(0,jumpForce));
            anim.SetTrigger("Jump");
        }
    }
    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            m_Attack = true;
            jumpAttack = true;
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            m_Slide = true;
        }
    }
    private void Flip(float horizontal)
    {

        if(horizontal>0 && !m_FacingRight || horizontal<0&&m_FacingRight)
        {
            m_FacingRight = !m_FacingRight;
            Vector2 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
    private void ResetValue()
    {
        m_Attack = false;
        m_Slide = false;
        jump = false;
        jumpAttack = false;
    }
    private bool IsGrounded()
    {
        foreach(Transform point in groundPoints)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position,groundRadius,whatIsGround);
            for(int i=0;i<colliders.Length;i++)
            {
                if(colliders[i].gameObject !=gameObject)
                {
                    anim.ResetTrigger("Jump");
                    anim.SetBool("IsLand", false);
                    return true;
                }
            }
        }
        return false;
    }
    private void HandleLayers()
    {
        if(!isGrounded)
        {
            anim.SetLayerWeight(1, 1);
        }
        else
        {
            anim.SetLayerWeight(1, 0);
        }
    }
}
