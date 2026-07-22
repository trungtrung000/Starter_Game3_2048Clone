# GAME DESIGN DOCUMENT
## 2048 Clone — Puzzle ghép số trên lưới

> Tài liệu được viết lại (reverse-engineer) từ source code Unity (C#) đã upload, dành cho **fresher Game Designer** dùng để học cách mô tả một game puzzle dựa trên luật chơi/thuật toán rõ ràng.

---

## 1. TỔNG QUAN (Game Overview)

| Mục | Nội dung |
|---|---|
| Tên project | Starter_Game3_2048Clone |
| Thể loại | Puzzle ghép số trên lưới (Grid-based Merge Puzzle) — cùng dòng với *2048* |
| Góc nhìn | 2D, giao diện UI toàn màn hình (UGUI) |
| Nền tảng | PC (build từ Unity, dùng bàn phím) |
| Engine | Unity (Universal Render Pipeline 2D) |
| Phiên game | Single player |
| Vòng lặp cốt lõi | Trượt toàn bộ ô số theo 1 trong 4 hướng → các ô cùng giá trị va vào nhau thì gộp làm đôi → sau mỗi lượt trượt, một ô số mới xuất hiện ngẫu nhiên → lặp lại đến khi đạt giá trị mục tiêu (thắng) hoặc lưới đầy không còn nước đi (thua) |

### 1.1 Câu chuyện một câu (One-line pitch)
Người chơi điều khiển một lưới ô vuông chứa các số, mỗi lần bấm phím hướng sẽ đẩy toàn bộ số về phía đó; hai số giống nhau chạm nhau sẽ gộp thành một số gấp đôi — mục tiêu là tạo ra được ô số mục tiêu trước khi lưới bị lấp đầy.

### 1.2 Người chơi mục tiêu
- Người chơi thích thể loại puzzle nhẹ nhàng, chơi được mọi lúc mọi nơi, không cần phản xạ nhanh mà cần tư duy sắp xếp không gian và lập kế hoạch trước vài nước đi (giống cờ vua ở quy mô nhỏ).

---

## 2. CORE GAMEPLAY LOOP

```
[Bắt đầu ván]
   → Lưới trống được sinh sẵn 2 ô số khởi điểm (giá trị 2)
   → Người chơi bấm 1 trong 4 phím hướng (W / A / S / D)
   → TOÀN BỘ ô số trên lưới trượt dồn về phía được chọn
   → Ô số nào trượt vào ô số khác CÙNG GIÁ TRỊ → gộp thành 1 ô có giá trị gấp đôi, cộng điểm
   → Sau khi trượt xong, hệ thống random sinh thêm 1 ô số mới (giá trị 2 hoặc 4) vào 1 ô trống bất kỳ
   → Kiểm tra:
        - Nếu vừa tạo ra ô đạt "giá trị mục tiêu" (Win Condition) → hiện màn hình Chiến thắng (có thể chọn Chơi tiếp)
        - Nếu lưới đầy VÀ không còn ô nào có thể gộp theo bất kỳ hướng nào → hiện màn hình Game Over
   → Lặp lại từ bước bấm phím hướng
```

**Vì sao loop này hoạt động (giải thích cho fresher):**
- Mỗi nước đi đều ảnh hưởng đến TOÀN BỘ lưới cùng lúc (không phải chỉ 1 ô) → người chơi buộc phải suy nghĩ về hậu quả tổng thể trước khi bấm, đây là nguồn gốc chính tạo ra độ sâu chiến thuật dù luật chơi cực kỳ đơn giản.
- Việc sinh ô số mới ngẫu nhiên sau MỖI lượt (không phải theo thời gian thực) giữ nhịp độ game hoàn toàn nằm trong tay người chơi — đặc trưng "turn-based" giúp game phù hợp chơi thư giãn, không áp lực thời gian.

---

## 3. LƯỚI & HỆ THỐNG Ô (Grid & Cell System)

### 3.1 Cấu trúc lưới
- Lưới hiện tại gồm **16 ô** (kích thước 4×4 — chuẩn của thể loại 2048), lưu trong mảng `allCells`.
- Mỗi ô (`Cell`) giữ tham chiếu trực tiếp tới 4 ô lân cận: `up`, `down`, `left`, `right` (thiết lập sẵn thủ công trong Editor thay vì tính theo toạ độ) — đây là cách tổ chức dữ liệu dạng **linked grid** (lưới liên kết), giúp thuật toán trượt/gộp chỉ cần "đi theo con trỏ" thay vì tính toán chỉ số hàng-cột.
- Mỗi ô có thể chứa hoặc không chứa một **Ô số (Fill)** — object con đại diện cho giá trị số đang nằm trong ô đó.

### 3.2 Ô số (Fill)
- Mỗi Fill có 1 giá trị số nguyên (`value`), luôn là luỹ thừa của 2 (2, 4, 8, 16...).
- Màu nền của ô số được chọn theo bảng màu `fillColors` dựa vào bậc luỹ thừa của giá trị (giá trị càng cao, màu càng khác biệt) — nguyên tắc UX: người chơi cần **nhận diện độ lớn của số bằng màu sắc** mà không cần đọc số, giúp quét nhanh bàn cờ.
- Khi 2 ô số gộp lại: giá trị nhân đôi, cộng điểm vào tổng điểm, đổi màu theo giá trị mới, và kiểm tra điều kiện thắng.
- Ô số di chuyển mượt về vị trí ô đích bằng nội suy vị trí theo thời gian thực (`MoveTowards`), không "dịch chuyển tức thời" — đây là chi tiết nhỏ nhưng quan trọng cho cảm giác chơi (game feel), vì 2048 gốc cũng dùng hiệu ứng trượt mượt tương tự.

### 3.3 Thuật toán trượt & gộp (giải thích cho fresher — đây là phần lõi của game)
Khi người chơi bấm 1 hướng, thuật toán xử lý **từng ô một, theo thứ tự từ ô gần biên đích nhất trở về xa nhất**:
1. Nếu ô hiện tại có chứa số → tìm ô số **gần nhất theo hướng di chuyển**.
   - Nếu số đó **cùng giá trị** → gộp (giá trị x2, xoá ô số cũ, cộng điểm).
   - Nếu số đó **khác giá trị** → chỉ đẩy số đó lại gần ô hiện tại thêm 1 bước (không gộp).
2. Nếu ô hiện tại **trống** → tìm ô số gần nhất theo hướng ngược lại để kéo về lấp chỗ trống, sau đó đệ quy lại chính ô này (đảm bảo không bỏ sót trường hợp cần kéo tiếp).
3. Toàn bộ quá trình lặp đệ quy dọc theo cả hàng/cột cho đến khi chạm biên lưới.

> **Lưu ý thiết kế quan trọng:** mỗi ô số chỉ được phép gộp **1 lần duy nhất mỗi lượt trượt** — đây là luật gốc bắt buộc của 2048 (tránh 4 ô giá trị 2 gộp liên hoàn thành 16 chỉ trong 1 lượt), fresher cần nắm rõ luật này khi mô tả hoặc balance lại thuật toán.

### 3.4 Kiểm tra hết nước đi (Game Over Check)
- Sau mỗi lượt, hệ thống kiểm tra từng ô: nếu ô đó có số VÀ có ít nhất 1 ô lân cận (trên/dưới/trái/phải) cùng giá trị hoặc còn ô trống → vẫn còn nước đi, game tiếp tục.
- Nếu quét toàn bộ 16 ô mà không tìm được bất kỳ nước đi khả dụng nào → hiện màn hình **Game Over**.

---

## 4. ĐIỀU KHIỂN (Controls)

| Phím | Hành động |
|---|---|
| W | Trượt toàn bộ số lên trên |
| A | Trượt toàn bộ số sang trái |
| S | Trượt toàn bộ số xuống dưới |
| D | Trượt toàn bộ số sang phải |

- Giữa 2 lượt bấm phím có **thời gian hồi tối thiểu 0.3 giây** (`slideTimer`) — để tránh người chơi bấm dồn dập gây lỗi hoạt ảnh chồng lấn giữa các lượt trượt, đồng thời tạo cảm giác nhịp độ vừa phải giữa các nước đi.
- Trong lúc màn hình Thắng hoặc Thua đang hiện, mọi input di chuyển bị khoá.

---

## 5. HỆ THỐNG SINH SỐ MỚI (Spawn System)

- Khi bắt đầu ván: sinh sẵn **2 ô số** giá trị 2 vào 2 vị trí ngẫu nhiên.
- Sau mỗi lượt trượt hợp lệ: hệ thống chọn ngẫu nhiên 1 ô trống để sinh thêm số mới, theo xác suất:

| Kết quả | Xác suất |
|---|---|
| Không sinh gì cả | 20% |
| Sinh ô số giá trị **2** | 60% |
| Sinh ô số giá trị **4** | 20% |

> **Ghi chú cho fresher:** xác suất "không sinh gì" (20%) là một lựa chọn thiết kế khác biệt so với bản 2048 gốc (bản gốc luôn sinh 1 ô sau mỗi lượt, tỉ lệ 90% ra số 2 / 10% ra số 4). Đây là điểm bạn nên cân nhắc balance lại nếu muốn độ khó sát với bản gốc hơn — việc thỉnh thoảng "không có gì mới xuất hiện" làm ván chơi dễ hơn vì lưới lấp đầy chậm hơn.

---

## 6. ĐIỂM SỐ (Scoring)

- Mỗi lần 2 ô số gộp thành công, điểm cộng thêm = **giá trị của ô số mới sau khi gộp** (ví dụ 2+2 gộp thành 4 → cộng 4 điểm, không phải cộng 2).
- Điểm được cộng dồn xuyên suốt cả ván, hiển thị trực tiếp trên HUD, không có giới hạn tối đa.
- Hiện tại điểm số **không được lưu lại** giữa các ván (không có bảng xếp hạng / điểm cao nhất) — đây là khoảng trống thường thấy ở bản "starter/clone" cơ bản, có thể bổ sung thêm nếu muốn tăng khả năng chơi lại.

---

## 7. ĐIỀU KIỆN THẮNG / THUA (Win & Lose Condition)

### 7.1 Thắng
- Có 1 giá trị đích (`winCon`, mặc định tương ứng ô **2048** theo đúng tên game) được cấu hình sẵn.
- Ngay khi bất kỳ ô số nào đạt đúng giá trị đích → hiện màn hình **Chiến Thắng**.
- Người chơi có thể bấm **"Chơi tiếp" (Keep Playing)** để đóng bảng thắng và tiếp tục ván hiện tại, thử đạt giá trị cao hơn (cơ chế "soft win" quen thuộc của 2048 gốc — thắng không có nghĩa là kết thúc, chỉ là một cột mốc).
- Bảng thắng chỉ hiện **1 lần duy nhất** mỗi ván (không hiện lại dù đạt thêm mốc cao hơn sau đó).

### 7.2 Thua
- Khi lưới đầy 16 ô VÀ không còn bất kỳ cặp ô liền kề nào cùng giá trị để gộp → hiện màn hình **Game Over**.
- Có nút **Restart** để tải lại toàn bộ scene, bắt đầu ván hoàn toàn mới.

---

## 8. UI / HUD

Theo cấu trúc code, màn chơi gồm:
- Lưới 4×4 hiển thị các ô số, mỗi ô số có màu nền + chữ số hiển thị giá trị (TextMeshPro/UI Text)
- Bảng điểm số hiện tại (Score Display)
- Bảng "Chiến Thắng" (Game Win Panel) — có nút Chơi tiếp
- Bảng "Thua Cuộc" (Game Over Panel) — có nút Restart

---

## 9. GHI CHÚ PHƯƠNG PHÁP LUẬN (dành riêng cho fresher)

Khi viết GDD cho một game puzzle dựa trên thuật toán/luật chơi rõ ràng (khác với game hành động nhiều số liệu như *auto-battler*), quy trình nên nhấn mạnh:
1. **Mô tả đúng luật chơi bằng lời trước khi mô tả bằng thuật toán** — người đọc GDD (đặc biệt là tester, marketing, hoặc designer khác) không phải lúc nào cũng đọc được code, nên phần luật chơi (mục 3.3) cần viết lại bằng ngôn ngữ tự nhiên, không chỉ diễn giải lại code từng dòng.
2. **Chỉ rõ những điểm khác biệt với "bản gốc" mà thể loại này ai cũng biết** (như mục 5) — khi làm clone, designer luôn cần so sánh trực tiếp với bản gốc để người đọc GDD biết đâu là chủ đích thiết kế mới, đâu là do chưa hoàn thiện.
3. Với game dạng lưới/thuật toán, nên **mô tả từng bước xử lý theo đúng thứ tự runtime thực thi** (nhận input → xử lý logic → cập nhật trạng thái → kiểm tra điều kiện kết thúc) thay vì liệt kê tính năng rời rạc — cách này giúp lập trình viên đối chiếu tài liệu với code dễ hơn nhiều.

---

*Tài liệu được tổng hợp từ source code thực tế của project `Starter_Game3_2048Clone-main` (3 script chính: `GameController.cs`, `Cell.cs`, `Fill.cs`).*
