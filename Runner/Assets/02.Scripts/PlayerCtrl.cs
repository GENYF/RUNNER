using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour {

    private float motionDelay;

    public bool isRight, isJumping = false, isCrounch = false;
    private bool isAir = false;
    public float speed;

    public GameObject dmgPanel;

    const int roadWidth = 1;
    const float offset = 0.1f;

    public Text scoreText, calorieText, timeText;
    public int score = 0, health = 5;

    public Image healthBar;

    public float calorie;
    private const float JUMP_CALORIE = 0.02f, CROUNCH_CALORIE = 0.01f;
    private int jumpCount = 0, crounchCount = 0, turnCount = 0, runTime = 0;
    private float nowTime;

    private Animator animator;
    public ObstacleManager obstacleManager;
    public SerialManager serialManager; 

    public bool isStop = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            score++;
        }
        else if (other.CompareTag("Obstacle"))
        {
            dmgPanel.SetActive(true);
            Invoke("DmgPannel", 0.4f);
            health--;
            if (health <= 0)
            {
                obstacleManager.isStop = isStop = true;
                GameManager.instance.GameOver(score, calorie);
            }
        }
        else if (other.CompareTag("Sinkhole"))
        {
            dmgPanel.SetActive(true);
            Invoke("DmgPannel", 0.4f);
            health = 0;
            obstacleManager.isStop = isStop = true;
            GameManager.instance.GameOver(score, calorie);
        }
    }

    void Start () {
        animator = GetComponent<Animator>();
	}

	void Update ()
    {
        if (isStop)
            return;

        Movement();
        InputCtrl();

        UICtrl();
    }

    private void DmgPannel()
    {
        dmgPanel.SetActive(false);
    }

    private void Movement()
    {
        if (isRight)
        {
            if (transform.position.z - offset > -roadWidth)
            {
                animator.SetBool("isRunning", true);
                transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
            else
                animator.SetBool("isRunning", false);
        }
        else
        {
            if (transform.position.z + offset < roadWidth)
            {
                animator.SetBool("isRunning", true);
                transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
            else
                animator.SetBool("isRunning", false);
        }
        if (isJumping && !isAir)
        {
            isAir = true;
            animator.SetTrigger("isJumping");
        }
        animator.SetBool("isCrounching", isCrounch);
    }

    private void InputCtrl()
    {
        if (motionDelay > 0)
        {
            motionDelay -= Time.deltaTime;
            return;
        }

        if (serialManager.connected)
        {
            if (serialManager.nowChar == 'L' && !isJumping)
            {
                //motionDelay = 0.3f;
                turnCount++;
                isRight = false;
            }
            else if (serialManager.nowChar == 'R' && !isJumping)
            {
                //motionDelay = 0.3f;
                turnCount++;
                isRight = true;
            }
            else if (serialManager.nowChar == 'S')
            {
                //motionDelay = 0.5f;
                jumpCount++;
                isJumping = true;
            }
            if (!isJumping)
            {
                if (serialManager.nowChar == 'D')
                {
                    isCrounch = true;
                    crounchCount++;
                }
                else
                    isCrounch = false;
            }
            else
                isCrounch = false;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                turnCount++;
            if (Input.GetKeyDown(KeyCode.RightArrow))
                turnCount++;
            if (Input.GetKeyDown(KeyCode.UpArrow))
                jumpCount++;
            if (Input.GetKeyDown(KeyCode.DownArrow))
                crounchCount++;

            if (Input.GetKey(KeyCode.LeftArrow) && !isJumping)
            {
                //motionDelay = 0.3f;
                isRight = false;
            }
            else if (Input.GetKey(KeyCode.RightArrow) && !isJumping)
            {
                //motionDelay = 0.3f;
                isRight = true;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                //motionDelay = 0.5f;
                isJumping = true;
            }
            if (!isJumping)
            {
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    isCrounch = true;
                }
                else
                    isCrounch = false;
            }
            else
                isCrounch = false;
        }
    }

    private void UICtrl()
    {
        scoreText.text = score.ToString();

        nowTime += Time.deltaTime;
        runTime = (int)nowTime;

        timeText.text = (runTime / 60).ToString("D2") + ":" + (runTime % 60).ToString("D2");

        calorie = JUMP_CALORIE * jumpCount + CROUNCH_CALORIE * crounchCount + CROUNCH_CALORIE * turnCount + runTime * 0.01f;
        calorieText.text = calorie.ToString("F2") + "kcal";

        healthBar.fillAmount = health * 0.2f;
    }

    public void StampGround()
    {
        isJumping =isAir = false;
    }

    public void ResetAll()
    {
        motionDelay = 0;
        isRight = isJumping = isCrounch = isAir = false;
        score = 0;
        calorie = 0;
        health = 5;
        jumpCount = crounchCount = runTime = 0;
        nowTime = 0;
        animator.Rebind();
        isStop = false;
    }
}
