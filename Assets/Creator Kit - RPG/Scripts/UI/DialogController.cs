// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DialogController.cs" company="">
//   
// </copyright>
// <summary>
//   The dialog controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using RPGM.Core;
using RPGM.Gameplay;
using UnityEngine;


namespace RPGM.UI
{
    /// <summary>
    /// The dialog controller.
    /// </summary>
    public class DialogController : MonoBehaviour
    {
        /// <summary>
        /// The dialog layout.
        /// </summary>
        public DialogLayout dialogLayout;

        /// <summary>
        /// The on button.
        /// </summary>
        public System.Action<int> onButton;

        /// <summary>
        /// The selected button.
        /// </summary>
        public int SelectedButton = 0;
        public int buttonCount = 0;

        SpriteButton[] buttons;
        Camera mainCamera;
        private readonly GameModel _model = Schedule.GetModel<GameModel>();
        SpriteUIElement spriteUIElement;

        /// <summary>
        /// The message.
        /// </summary>
        private string _message;

        /// <summary>
        /// The focus button.
        /// </summary>
        /// <param name="direction">
        /// The direction.
        /// </param>
        public void FocusButton(int direction)
        {
            if (buttonCount > 0)
            {
                if (this.SelectedButton < 0) this.SelectedButton = 0;
                buttons[this.SelectedButton].Exit();
                this.SelectedButton += direction;
                this.SelectedButton = Mathf.Clamp(this.SelectedButton, 0, buttonCount - 1);
                buttons[this.SelectedButton].Enter();
            }
        }

        public void SelectActiveButton()
        {
            if (buttonCount > 0)
            {
                if (this.SelectedButton >= 0)
                {
                    _model.input.ChangeState(InputController.State.CharacterControl);
                    buttons[this.SelectedButton].Click();
                    this.SelectedButton = -1;
                }
            }
            else
            {
                //there are no buttons, just Hide when required.
                _model.input.ChangeState(InputController.State.CharacterControl);
                Hide();
            }
        }

        public void Show(SpriteRenderer contextSprite, string text)
        {
            var position = contextSprite.transform.position;
            position.x -= contextSprite.size.x;
            position.y -= contextSprite.size.y * 0.5f;
            position.y += dialogLayout.spriteRenderer.size.y;
            Show(position, text);
        }

        public void SetButton(int index, string text)
        {
            var d = dialogLayout;
            d.SetButtonText(index, text);
            buttonCount = Mathf.Max(buttonCount, index + 1);
        }

        public void Show(Vector3 position, string text)
        {
            var d = dialogLayout;
            d.gameObject.SetActive(true);
            d.SetText(text);
            SetPosition(position);
            _model.input.ChangeState(InputController.State.DialogControl);
            buttonCount = 0;
            this.SelectedButton = -1;
        }

        public void Show(Vector3 position, string text, string buttonA)
        {
            UserInterfaceAudio.OnShowDialog();
            var d = dialogLayout;
            d.gameObject.SetActive(true);
            d.SetText(text, buttonA);
            SetPosition(position);
            _model.input.ChangeState(InputController.State.DialogControl);
            buttonCount = 1;
            this.SelectedButton = -1;
        }

        public void Show(Vector3 position, string text, string buttonA, string buttonB)
        {
            UserInterfaceAudio.OnShowDialog();
            var d = dialogLayout;
            d.gameObject.SetActive(true);
            d.SetText(text, buttonA, buttonB);
            SetPosition(position);
            _model.input.ChangeState(InputController.State.DialogControl);
            buttonCount = 2;
            this.SelectedButton = -1;
        }

        void SetPosition(Vector3 position)
        {
            var screenPoint = mainCamera.WorldToScreenPoint(position);
            position = spriteUIElement.camera.ScreenToViewportPoint(screenPoint);
            spriteUIElement.anchor = position;
        }

        public void Show(Vector3 position, string text, string buttonA, string buttonB, string buttonC)
        {
            UserInterfaceAudio.OnShowDialog();
            var d = dialogLayout;
            d.gameObject.SetActive(true);
            d.SetText(text, buttonA, buttonB, buttonC);
            SetPosition(position);
            _model.input.ChangeState(InputController.State.DialogControl);
            buttonCount = 3;
            this.SelectedButton = -1;
        }

        public void Hide()
        {
            UserInterfaceAudio.OnHideDialog();
            dialogLayout.gameObject.SetActive(false);
        }

        public void SetIcon(Sprite icon) => dialogLayout.SetIcon(icon);

        void OnButton(int index)
        {
            if (onButton != null) onButton(index);
            onButton = null;
        }

        void Awake()
        {
            dialogLayout.gameObject.SetActive(false);
            buttons = dialogLayout.buttons;
            dialogLayout.buttonA.onClickEvent += () => OnButton(0);
            dialogLayout.buttonB.onClickEvent += () => OnButton(1);
            dialogLayout.buttonC.onClickEvent += () => OnButton(2);
            spriteUIElement = GetComponent<SpriteUIElement>();
            mainCamera = Camera.main;
        }
    }
}