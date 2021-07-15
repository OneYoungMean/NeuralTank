using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControll : MonoBehaviour //OYM:操控坦克行为的类
{
    // Start is called before the first frame update
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private ParticleSystem[] m_particleSystems; // References to all the particles systems used by the Tanks
    public float leftWheel;
    public float rightWheel;
    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;

        // Also reset the input values.
        leftWheel = 0f;
        rightWheel = 0f;

        // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
        // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
        // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke
        m_particleSystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < m_particleSystems.Length; ++i)
        {
            m_particleSystems[i].Play();
        }
    }

    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;

        // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
        for (int i = 0; i < m_particleSystems.Length; ++i)
        {
            m_particleSystems[i].Stop();
        }
    }
    public void Move()
    {
        m_Rigidbody.AddForceAtPosition(leftWheel * transform.forward * 20, transform.position - transform.right * 5);
        m_Rigidbody.AddForceAtPosition(rightWheel * transform.forward * 20, transform.position + transform.right * 5);
    }
}
