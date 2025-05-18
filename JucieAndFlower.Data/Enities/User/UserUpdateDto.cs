namespace JucieAndFlower.Data.Enities.User
{
    public class UserUpdateDto
    {
        // Thông tin cá nhân
        public string FullName { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public DateOnly? BirthDate { get; set; }

        // Thay đổi mật khẩu (không bắt buộc)
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
