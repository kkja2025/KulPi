using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GridPlayer : MonoBehaviour
{
    private int skillCharge = 0;
    private int damage = 0;

    [SerializeField] private int maxHP;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private Button upButton;
    [SerializeField] private Button downButton;

    [SerializeField] private Transform gridContainer;
    [SerializeField] private float movementDuration = 0.2f;

    private int totalRows = 5;
    private int currentRow = 2;
    private bool isMoving = false;

    private void Start()
    {
        upButton.onClick.AddListener(OnMoveUpButtonPressed);
        downButton.onClick.AddListener(OnMoveDownButtonPressed);
    }

    private void OnMoveUpButtonPressed()
    {
        if (isMoving || currentRow <= 0) return;
        currentRow--;
        SetPlayerPosition();
        CheckTileInteraction();
    }

    private void OnMoveDownButtonPressed()
    {
        if (isMoving || currentRow >= totalRows - 1) return;
        currentRow++;
        SetPlayerPosition();
        CheckTileInteraction();
    }

    private void SetPlayerPosition()
    {
        int tileIndex = currentRow;
        Transform targetTile = gridContainer.GetChild(tileIndex);

        StartCoroutine(MoveToPosition(targetTile.position));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isMoving = true;
        Vector3 startPosition = transform.position;
        float elapsedTime = 0;

        while (elapsedTime < movementDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / movementDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
    }

    public int GetCurrentRow()
    {
        return currentRow;
    }

    public void CheckTileInteraction()
    {
        Transform tileTransform = gridContainer.GetChild(currentRow);
        if (tileTransform != null)
        {
            Image tileImage = tileTransform.GetComponent<Image>();
            if (tileImage != null)
            {
                if (tileImage.color == Color.green)
                {
                    ChargeSkill();
                }
                else if (tileImage.color == Color.red)
                {
                    TakeDamage();
                }
            }
        }
    }


    private void ChargeSkill()
    {
        skillCharge++;
        if (skillCharge >= 5)
        {
            BattleManagerSakim bossBattleManager = BattleManager.Singleton as BattleManagerSakim;
            if (bossBattleManager != null)
            {
                bossBattleManager.ShowUltimateButton();
            }
        }
    }

    public void ResetCharge()
    {
        skillCharge = 0;
    }

    private void TakeDamage()
    {
        damage++;
        if (healthBarFill != null)
        {
            float healthPercentage = (float)(maxHP - damage) / maxHP;
            healthBarFill.fillAmount = healthPercentage;
        }
    }
}
