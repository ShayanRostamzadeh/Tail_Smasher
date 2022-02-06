using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool isMoving = false;
    private Vector3 moveForce;
    [Range(0f, 1f)] public float driftFactor;
    public float reverseFactor;
    public float forwardForce;
    public float rotationFactor = 50f;

    // Do not change the player health initial value
    [HideInInspector] public float playerHealth = 100f;
    
    public bool forwardForceEnable = true;
    private bool turnRight = false;
    private bool turnLeft = false;

    //private float carTiltDelay = 0f;
    
    [SerializeField] private Transform centerOfMass;
    [HideInInspector] public GameObject[] frontLights;
    [HideInInspector] public GameObject[] rearLights;
    public Animator animator;
    private Rigidbody playerRigidbody;
    public GameObject carBodyFractured;
    public GameController _gameController;
    
    private AudioSource audioSource;
    public AudioClip carDriftSoundClip;
    public ParticleSystem smokePS;
    public ParticleSystem firePS;

    private void Start()
    {
        // Physics
        playerRigidbody = GetComponent<Rigidbody>();
        // Audio 
        audioSource = GetComponent<AudioSource>();

        // todo: how to append one array to another????????
        if (!_gameController.isNight)
        {
            frontLights = GameObject.FindGameObjectsWithTag("FrontLight");
            rearLights = GameObject.FindGameObjectsWithTag("RearLight");
            foreach (GameObject light in frontLights)
                light.SetActive(false);
            foreach (GameObject light in rearLights)
                light.SetActive(false);
        }
    }
    
    private void FixedUpdate()
    {   
        // initialization

        playerRigidbody.centerOfMass = centerOfMass.localPosition;

        #region Movement
        // The player continuously goes forward without the control of user 
        if(forwardForceEnable && isMoving)
            ForceForward();
        
        // Checking if UI buttons are pressed to steer accordingly 
        Steer();
        Reverse();
        #endregion Movement

        #region ParticleSystem
        // Activate smoke particle system when health reaches 50%
        PlayParticleSystem();
        #endregion ParticleSystem

    }
    private void PlayParticleSystem()
    { 
        if(20f < playerHealth && playerHealth < 50f)
        {
            firePS.gameObject.SetActive(false);
            smokePS.gameObject.SetActive(true);
            smokePS.Play();
        }
        else if(playerHealth <= 20f )
        {
            firePS.gameObject.SetActive(true);
            smokePS.gameObject.SetActive(false);
        }
        else
        {
            smokePS.Stop();
            firePS.Stop();
        }
    }

    #region Button Controller
    public void TurnRight() => turnRight = true;
    public void TurnLeft() => turnLeft = true;
    
    public void ButtonPointerUp()
    {
        turnRight = false;
        turnLeft = false;
    }//ButtonPointerUp
    #endregion
    
    #region Impact
    
    // private void OnTriggerEnter(Collider target)
    // {
    //     if (target.gameObject.CompareTag("Enemy"))
    //     {
    //         DeathSequence();
    //     }
    // }

    private void OnCollisionEnter(Collision target)
    {
        switch (target.gameObject.tag)
        {
            case "Enemy":
                GetDamage(target.gameObject);
                if (playerHealth <= 0f) 
                {
                    DeathSequence(); 
                }
                break;
            case "PlayGround":
                isMoving = true;
                break;

            case "Animal":
                _gameController.animalsNum--;
                target.gameObject.GetComponent<AnimalController>().isAlive = false;
                target.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                target.gameObject.GetComponent<Animator>().SetTrigger("dies");
                if(!target.gameObject.GetComponent<AnimalController>().isAlive)
                {
                    target.gameObject.GetComponent<GameController>().InstantiateAnimal("Animal");
                }
                break;

            default:
                //isMoving = false;
                break;
        }
    }
    
    private void OnCollisionExit(Collision other) 
    {
        if(other.gameObject.tag == "PlayGround")
            isMoving = false;
    }
    private void GetDamage(GameObject enemy)
    {
        string name = enemy.gameObject.GetComponent<EnemyFollow>().enemyName;
        switch (name)
        {
            case "hitter":
                playerHealth -= 30f;
                break;
            case "Shooter":
                playerHealth -= 15f;
                break;
            case "exploder":
                playerHealth -= 70f;
                break;
            default:
                break;
        }
    }

    private void DeathSequence()
    {
        forwardForceEnable = false;
        Instantiate(carBodyFractured, transform.position, Quaternion.identity);
        carBodyFractured.GetComponent<Rigidbody>().AddExplosionForce(10000f, transform.position, 100f);
        Destroy(gameObject);
    }
    #endregion
    
    #region Movement
    private void ForceForward()
    {
        
        if(forwardForce != 0 && isMoving)
        {
        //Vector3 movement = transform.forward * (forwardForce * Time.deltaTime);
        moveForce += transform.forward * forwardForce * Time.deltaTime;

        // restricting the forward applied force to 40f
        moveForce = Vector3.ClampMagnitude(moveForce, 40f);

        // manipulating the drifting factor
        moveForce = Vector3.Lerp(moveForce.normalized, transform.forward, driftFactor * Time.deltaTime) * moveForce.magnitude;
        
        //playerRigidbody.MovePosition(moveForce);
        playerRigidbody.velocity = moveForce;
        //playerRigidbody.AddForce(moveForce * 10f, ForceMode.Impulse);
        Debug.DrawRay(playerRigidbody.position, moveForce, Color.red);
        //transform.position += moveForce * Time.deltaTime;
        }
    } //ForceForward()
    
    public void Reverse()
    {
        if (turnLeft && turnRight && isMoving)
        {
            forwardForceEnable = false;
            Vector3 movement = transform.forward * (forwardForce * Time.deltaTime);
            playerRigidbody.velocity = -movement * reverseFactor;
            foreach (var light in rearLights)
            {
                if (!light.activeSelf)
                    light.SetActive(true);
            }
        }
        // todo remove this section in the final product
        else if (Input.GetKey(KeyCode.Space))
        {
            forwardForceEnable = false;
            Vector3 movement = transform.forward * (forwardForce * Time.deltaTime);
            playerRigidbody.velocity = -movement * reverseFactor;
            foreach (var light in rearLights)
            {
                if (!light.activeSelf)
                    light.SetActive(true);
            }
        }
        else
        {
            forwardForceEnable = true;
            foreach (var light in rearLights)
            {
                if (light.activeSelf)
                    light.SetActive(false);
            }
        }

    }//Reverse()

    private void Steer()
    {
        if (turnLeft && forwardForceEnable)
        {
            animator.SetBool("turnLeft", true);
            transform.Rotate(-Vector3.up * rotationFactor * Time.deltaTime, Space.World);
            //playerRigidbody.MoveRotation(Quaternion.Euler(-Vector3.up * (Time.deltaTime * rotationFactor)) * transform.rotation);
            audioSource.PlayOneShot(carDriftSoundClip);
            // carBody.transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, -Vector3.forward * 8, carTiltDelay));
            // carTiltDelay += 0.1f;
        }
        else if (turnRight && forwardForceEnable)
        {
            animator.SetBool("turnRight", true);
            transform.Rotate(Vector3.up * rotationFactor * Time.deltaTime, Space.World);
            //playerRigidbody.MoveRotation(Quaternion.Euler(Vector3.up * (Time.deltaTime * rotationFactor)) * transform.rotation);
            audioSource.PlayOneShot(carDriftSoundClip);
            // carBody.transform.localRotation = Quaternion.Euler(Vector3.Lerp(Vector3.zero, Vector3.forward * 8, carTiltDelay));
            // carTiltDelay += 0.1f;
        }
        else
        {
            animator.SetBool("turnLeft", false);
            animator.SetBool("turnRight", false);
            //carBody.transform.localRotation = Quaternion.identity;
            //carTiltDelay -= 0.1f;
        }
    } //Steer()

    #endregion

}
