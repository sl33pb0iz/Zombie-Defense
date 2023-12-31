using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unicorn.UI;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;
using Unicorn;
using System.Text.RegularExpressions;
using UnityEngine.Events;

public class UiPopupUsername : UICanvas
{
    [SerializeField] private int maxUsernameLength = 10;

    [SerializeField] private Button okButton;

    [SerializeField] private TMP_InputField display;

    [SerializeField] private GameObject txtNotice;

    private PlayerDataManager PlayerDataManager => PlayerDataManager.Instance;

    [HideInInspector] public UnityAction ChangeUsernameAction;  

    public void Start()
    {
        display.characterLimit = maxUsernameLength;
        display.text = GenerateRandomUsername();
    }

    private string GenerateRandomUsername()
    {
        // Tạo 4 chữ số ngẫu nhiên
        int randomDigits = Random.Range(0, 10000);
        string randomUsername = "USER" + randomDigits.ToString("D4"); // D4 đảm bảo luôn có 4 chữ số, nếu số ngẫu nhiên ít hơn 4 chữ số, sẽ tự động thêm số 0 vào đầu
        PlayerDataManager.SetUsername(randomUsername);
        return randomUsername;
    }
    
    public void OnEnable()
    {
        okButton.onClick.AddListener(OnClickButtonOK);
        display.onEndEdit.AddListener(OnEndEditInputField); // Đăng ký sự kiện khi người chơi kết thúc nhập liệu tên người dùng
    }

    private void OnDisable()
    {
        okButton.onClick.RemoveListener(OnClickButtonOK);
        display.onEndEdit.RemoveListener(OnEndEditInputField);
    }

    public override void Show(bool _isShown, bool isHideMain = true)
    {
        base.Show(_isShown, isHideMain);

        if (!_isShown)
        {
            return;
        }
    }

    public void CreateUsername()
    {
        string username = display.text;
        // Kiểm tra xem tên người dùng có hợp lệ không và xử lý việc lưu tên người dùng vào hệ thống
        if (IsValidUsername(username))
        {
            // Đánh dấu là đã mở game lần đầu tiên, để lần sau không hiển thị nữa
            PlayerDataManager.SetChangeableUsername(false);

            // Xử lý việc lưu tên người dùng vào hệ thống ở đây
            PlayerDataManager.SetUsername(username);
            Show(false);

            // Đóng popup sau khi lưu tên người dùng thành công
        }
        else
        {
            txtNotice.SetActive(true);
            // Hiển thị thông báo lỗi nếu tên người dùng không hợp lệ (ví dụ: rỗng)
        }
    }

    private bool IsValidUsername(string username)
    {
        // Sử dụng Regular Expression để kiểm tra tính hợp lệ của tên người dùng
        // Pattern [^\p{L}\p{N}\p{P}\p{Z}]: cho phép ký tự tiếng Anh (có dấu và không dấu), chữ số và các ký tự đặc biệt (trừ khoảng trắng)
        return !string.IsNullOrWhiteSpace(username) && Regex.IsMatch(username, @"^[^\p{Z}]*$");
    }

    private void OnClickButtonOK()
    {
        
        // Gọi phương thức tạo tên người dùng khi người chơi nhấn nút OK
        CreateUsername();
        ChangeUsernameAction?.Invoke(); 
    }

    private void OnEndEditInputField(string inputText)
    {
        // Xử lý sự kiện khi người chơi kết thúc nhập liệu tên người dùng (có thể gọi hàm kiểm tra tính hợp lệ của tên người dùng ở đây)
        // Ví dụ:
        if (string.IsNullOrEmpty(inputText))
        {
            Debug.Log("Vui lòng nhập tên người dùng!");
        }
    }
}