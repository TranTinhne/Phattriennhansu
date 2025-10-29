// Hiển thị thông báo nhỏ khi người dùng click vào dòng dữ liệu
document.addEventListener("DOMContentLoaded", function () {
    const rows = document.querySelectorAll("tbody tr:not(.table-section)");

    rows.forEach(row => {
        row.addEventListener("click", () => {
            const courseName = row.children[3]?.innerText;
            alert(`Bạn đã chọn: ${courseName}`);
        });
    });
});
