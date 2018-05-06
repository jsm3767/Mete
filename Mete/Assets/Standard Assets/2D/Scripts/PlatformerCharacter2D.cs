using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 20f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_AirSpeed = 10f;
         private float m_JumpForce = 1000f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded

        const float onWallRadius = .14f;

        [SerializeField]
        private bool onWallRight;
        [SerializeField]
        private bool onWallLeft;

        private bool reboundLeft;
        private bool reboundRight;
        private float reboundLeftTimer;
        private float reboundRightTimer;

        //sound
        private AudioSource audioSource;

        public AudioClip jumpSound;
        public AudioClip doubleJumpSound;
        public AudioClip runSound;
        public AudioClip wallSlideSound;
        public AudioClip loseSound;

        private Transform leftCheck;
        private Transform rightCheck;
        private Transform artTransform;
        private Vector2 velocityLastUpdate = new Vector2(0,0);
        public SpriteRenderer jumpIndicator;

        private float reboundForce = 1400.0f;

        [SerializeField] private int nJumpsLeft;
        [SerializeField] private float cantWallSlideTimer = 0.0f;
        [SerializeField] private float cantJumpTimer = 0.0f;

        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.
        

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");

            leftCheck = transform.Find("LeftCheck");
            rightCheck = transform.Find("RightCheck");

            artTransform = transform.Find("Art");

            audioSource = GetComponent<AudioSource>();
            m_Anim = GetComponentInChildren<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }


        private void FixedUpdate()
        {
            m_Grounded = false;
            onWallRight = false;
            onWallLeft = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                { 
                    m_Grounded = true;
                    nJumpsLeft = 1;
                }
            }

            if(cantWallSlideTimer <= 0.0f)
            { 
                Collider2D[] left = Physics2D.OverlapCircleAll(leftCheck.position, onWallRadius);
                Collider2D[] right = Physics2D.OverlapCircleAll(rightCheck.position, onWallRadius);
                for (int i = 0; i < left.Length; i++)
                {
                    if (left[i].gameObject != gameObject)
                    {
                        if( !onWallLeft)
                        { 
                            onWallLeft = true;
                            m_Rigidbody2D.AddForce(new Vector2(0.0f, Mathf.Abs(velocityLastUpdate.x * 2.0f)));
                        }
                    }
                    
                }
                for (int i = 0; i < right.Length; i++)
                {
                    if (right[i].gameObject != gameObject)
                    {
                        if (!onWallRight)
                        {
                            onWallRight = true;
                            m_Rigidbody2D.AddForce(new Vector2(0.0f, Mathf.Abs(velocityLastUpdate.x * 2.0f)));
                        }
                    }
                }
            }

            if ( (audioSource.clip == runSound && audioSource.isPlaying))
            { 
                audioSource.pitch = 1.5f*(GetComponent<Rigidbody2D>().velocity.magnitude / 20.0f);
            }

            if( !m_Grounded && audioSource.clip == runSound && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            if (m_Grounded && ( !((audioSource.clip == runSound || audioSource.clip == jumpSound) && audioSource.isPlaying) ) )
            {
                audioSource.volume = 1.0f;

                audioSource.clip = runSound;
                audioSource.loop = true;
                audioSource.Play();
            }

            if ( !m_Grounded && (onWallLeft || onWallRight) )
            {
                //mess with wall velocity if wanted
                //m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, -1.0f);

                //audioSource.clip = wallSlideSound;
                //audioSource.Play();
            }
            else
            {

            }

            if(cantWallSlideTimer > 0.0f)
            {
                cantWallSlideTimer -= Time.deltaTime;
            }

            if (cantJumpTimer > 0.0f)
            {
                cantJumpTimer -= Time.deltaTime;
            }

            if(reboundLeftTimer > 0.0f)
            {
                reboundLeftTimer -= Time.deltaTime;
            }
            if (reboundRightTimer > 0.0f)
            {
                reboundRightTimer -= Time.deltaTime;
            }
            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);

            velocityLastUpdate = m_Rigidbody2D.velocity;

            if(nJumpsLeft > 0 )
            {
                jumpIndicator.enabled = true;
            }
            else
            {
                jumpIndicator.enabled = false;
            }
        }


        public void Move(float move, bool crouch, bool jump)
        {
            // If crouching, check to see if the character can stand up
            //if (!crouch && m_Anim.GetBool("Crouch"))
            //{
            //    // If the character has a ceiling preventing them from standing up, keep them crouching
            //    if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            //    {
            //        crouch = true;
            //    }
            //}

            // Set whether or not the character is crouching in the animator
            //m_Anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                //move = (crouch ? move*m_CrouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                //Todo change this

                if( !m_Grounded)
                {
                    //Debug.Log("move: " + move);
                    float airmove = 0;
                    if (move > .1)
                        airmove = 1;
                    else if (move < -.1)
                        airmove = -1;

                    m_Rigidbody2D.velocity = new Vector2(Mathf.Clamp(m_Rigidbody2D.velocity.x + (airmove*2.0f), -m_AirSpeed, m_AirSpeed), m_Rigidbody2D.velocity.y);

                }
                else
                {
                    
                    m_Rigidbody2D.velocity = new Vector2( (move * m_MaxSpeed), m_Rigidbody2D.velocity.y);
                }

                //m_Rigidbody2D.AddForce(new Vector2(move * 1000,0.0f));
                //m_Rigidbody2D.velocity = new Vector2(Mathf.Clamp(m_Rigidbody2D.velocity.x * .9f, -4.0f, 4.0f), m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            // If the player should jump...
            if (m_Grounded && jump && m_Anim.GetBool("Ground") && cantJumpTimer<=0.0f)
            {
                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", false);
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0.0f);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));

                audioSource.pitch = 1.0f;
                audioSource.volume = .2f;
                audioSource.loop = false;
                audioSource.clip = jumpSound;
                audioSource.Play();
                cantJumpTimer = .1f;
            }
            //wall jump
            else if( (onWallLeft || onWallRight) && jump && !m_Grounded && cantJumpTimer <= 0.0f)
            {
                cantWallSlideTimer = .3f;
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0.0f);
                if(onWallLeft)
                {
                    reboundRight = true;
                    m_Rigidbody2D.AddForce(new Vector2(reboundForce, m_JumpForce));
                    reboundRightTimer = .3f;
                }
                else if( onWallRight)
                {
                    reboundLeft = true;
                    m_Rigidbody2D.AddForce(new Vector2(-reboundForce, m_JumpForce));
                    reboundLeftTimer = .3f;
                }
                cantJumpTimer = .1f;

                audioSource.volume = .2f;

                audioSource.clip = jumpSound;
                audioSource.pitch = 1.0f;
                audioSource.loop = false;
                audioSource.Play();

                onWallLeft = false;
                onWallRight = false;
            }
            //air jump
            else if(!m_Grounded && !onWallLeft && !onWallRight && jump && nJumpsLeft > 0 && cantJumpTimer <= 0.0f)
            {
                nJumpsLeft--;
                m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, 0.0f);
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                cantJumpTimer = .1f;
                audioSource.volume = .2f;

                audioSource.clip = doubleJumpSound;
                audioSource.pitch = 1.0f;
                audioSource.loop = false;
                audioSource.Play();

            }
        }


        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            //Multiply the player's x local scale by -1.
            Vector3 theScale = artTransform.localScale;
            theScale.x *= -1;
            artTransform.localScale = theScale;
        }

        public void PlaySound(float volume, float pitch, bool loop, AudioClip clip)
        {
            audioSource.pitch = pitch;
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
}
