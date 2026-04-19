# DataTables State Saving - Hướng Dẫn Chi Tiết

## 📋 Giới Thiệu

**State Saving** cho phép DataTables tự động ghi nhớ trạng thái của bảng (trang hiện tại, độ dài trang, sắp xếp, tìm kiếm, bộ lọc) vào localStorage của trình duyệt. Khi người dùng quay lại trang, bảng sẽ khôi phục đúng trạng thái trước đó.

---

## 🎯 Tính Năng State Saving Bao Gồm

| Thành phần | Mô tả | Ví dụ |
|-----------|-------|-------|
| **Current Page** | Trang hiện tại đang xem | Trang 3 |
| **Page Length** | Số dòng trên một trang | 25 hàng/trang |
| **Sort Column** | Cột nào đang sắp xếp | Sắp xếp theo "Price" |
| **Sort Direction** | Chiều sắp xếp (asc/desc) | Giảm dần (desc) |
| **Search** | Từ khóa tìm kiếm | "Apple" |
| **Column Filters** | Giá trị lọc cho từng cột | Category = "Fruit" |
| **Timestamp** | Thời điểm lưu trạng thái | 2024-01-15 14:30:45 |

---

## 🚀 Kích Hoạt State Saving - Cơ Bản

### 1️⃣ Cách Đơn Giản Nhất

```javascript
$('#myTable').DataTable({
    stateSave: true  // ← Bật lưu trạng thái
});
```

**Kết quả:**
- ✅ Tự động lưu trạng thái vào `localStorage`
- ✅ Tồn tại **vĩnh viễn** cho đến khi user xóa cache trình duyệt
- ✅ Khi reload trang, DataTables sẽ khôi phục trạng thái cũ

---

## ⏱️ Thiết Lập Thời Gian Hết Hạn (State Duration)

### `stateDuration` - Kiểm Soát Thời Gian Lưu

```javascript
$('#myTable').DataTable({
    stateSave: true,
    stateDuration: 60 * 60 * 2  // 2 giờ (tính bằng giây)
});
```

### Các Giá Trị Phổ Biến

| Giá trị | Ý nghĩa | Behavior |
|--------|---------|----------|
| **Không set** | Mặc định | localStorage (permanent) |
| **-1** | Session-only | Mất khi đóng tab/trình duyệt |
| **0** | Permanent | Sử dụng localStorage (không timeout) |
| **3600** | 1 giờ | localStorage + 1 giờ expire |
| **86400** | 1 ngày | localStorage + 1 ngày expire |
| **604800** | 1 tuần | localStorage + 1 tuần expire |

### Ví Dụ Thực Tế

```javascript
// 🔴 Session-only (mất khi đóng trình duyệt)
$('#tempTable').DataTable({
    stateSave: true,
    stateDuration: -1
});

// 🟢 Permanent (tồn tại lâu dài)
$('#permanentTable').DataTable({
    stateSave: true
    // stateDuration không set = permanent
});

// 🟡 24 giờ
$('#dailyTable').DataTable({
    stateSave: true,
    stateDuration: 60 * 60 * 24  // 24 hours
});

// 🔵 2 giờ
$('#shortTable').DataTable({
    stateSave: true,
    stateDuration: 60 * 60 * 2   // 2 hours
});
```

---

## 🔌 State Storage Locations

### localStorage vs sessionStorage

```javascript
// ==================== localStorage ====================
// ✅ Persistent across browser restarts
// ✅ Shared across tabs in same domain
// ❌ User có thể xóa bằng "Clear Cache"
$('#table').DataTable({
    stateSave: true,
    stateDuration: 0  // hoặc không set
});

// ==================== sessionStorage ====================
// ✅ Automatically cleared when tab closes
// ❌ Lost when browser closes
// ❌ Not shared across tabs
$('#table').DataTable({
    stateSave: true,
    stateDuration: -1
});
```

---

## 🛠️ Advanced: Tích Hợp với Individual Column Filters

### Vấn Đề Khi Kết Hợp

Khi dùng **State Saving + Individual Column Filters**, có một vấn đề:
- ✅ DataTables nhớ giá trị filter
- ❌ Nhưng giao diện (Dropdown/Select) bị reset về mặc định

### Giải Pháp: Restore UI State

```javascript
initComplete: function () {
    var api = this.api();
    var state = api.state.loaded();  // ← Lấy trạng thái đã lưu

    api.columns().every(function (colIndex) {
        var column = this;
        var header = $(column.header());

        // Tạo dropdown filter
        var $filter = $('<div class="dropdown">...</div>');
        header.append($filter);

        // ===== RESTORE SAVED STATE =====
        if (state && state.columns && state.columns[colIndex]) {
            var colState = state.columns[colIndex];

            if (colState.search && colState.search.search) {
                // Xóa ký tự regex ^ và $ để lấy giá trị gốc
                var savedValue = colState.search.search.replace(/^\^|\$$/g, '');

                // Cập nhật giao diện
                $filter.find('.filter-option').each(function () {
                    if ($(this).data('value') === savedValue) {
                        $(this).addClass('active');
                        $(this).closest('.dropdown').find('.filter-icon').addClass('text-warning');
                    }
                });
            }
        }
        // ================================

        // Xử lý click
        $filter.find('.filter-option').on('click', function (e) {
            e.preventDefault();
            var val = $(this).data('value');
            column.search(val ? '^' + $.fn.dataTable.util.escapeRegex(val) + '$' : '', true, false).draw();
        });
    });
}
```

### Giải Thích Chi Tiết

```javascript
// Step 1: Lấy trạng thái đã lưu
var state = api.state.loaded();

// Step 2: Kiểm tra xem có trạng thái cột không
if (state && state.columns && state.columns[colIndex]) {
    var colState = state.columns[colIndex];

    // Step 3: Lấy search value (có format regex ^value$)
    if (colState.search && colState.search.search) {
        var rawSearch = colState.search.search;
        // Input: "^Fruit$"
        // Output: "Fruit"
        var savedValue = rawSearch.replace(/^\^|\$$/g, '');

        // Step 4: Cập nhật UI
        // - Thêm class 'active' vào option tương ứng
        // - Highlight icon filter
    }
}
```

---

## 📊 State Callbacks - Lắng Nghe Sự Thay Đổi

### `stateLoaded.dt` - Khi State Được Load

```javascript
$('#myTable').on('stateLoaded.dt', function (e, settings, data) {
    console.log('State loaded from storage:', data);
    // Cập nhật UI, log thông tin, v.v.
});
```

### `stateSaved.dt` - Khi State Được Save

```javascript
$('#myTable').on('stateSaved.dt', function (e, settings, data) {
    console.log('State saved to storage:', data);
    // Có thể dùng để hiện toast notification
    // "Table state saved successfully"
});
```

### Ví Dụ Thực Tế

```javascript
var table = $('#myTable').DataTable({
    stateSave: true,
    stateDuration: 60 * 60 * 24
});

// Lắng nghe state changes
table.on('stateLoaded.dt', function (e, settings, data) {
    console.log('✅ Previous state restored');
    console.log('Current page:', (data.start / data.length) + 1);
    console.log('Sort:', data.order);
});

table.on('stateSaved.dt', function (e, settings, data) {
    console.log('💾 State saved');
    // Có thể hiên notification
    showNotification('Table state saved!', 'success');
});
```

---

## 🧹 Clear/Reset State

### Xóa State từ localStorage

```javascript
// Method 1: Xóa key cụ thể
function clearTableState(tableId) {
    var stateKey = 'DataTables_' + tableId + '_' + window.location.pathname;
    localStorage.removeItem(stateKey);
    location.reload();  // Reload page để thấy effect
}

// Sử dụng
clearTableState('stateSavingTable');

// Method 2: Xóa tất cả DataTables state
function clearAllDataTablesState() {
    for (let key in localStorage) {
        if (key.startsWith('DataTables_')) {
            localStorage.removeItem(key);
        }
    }
    location.reload();
}
```

### Button Clear State

```html
<button id="clearStateBtn" class="btn btn-warning">
    <i class="bi bi-arrow-clockwise"></i> Clear Saved State
</button>
```

```javascript
$('#clearStateBtn').on('click', function () {
    if (confirm('Are you sure? This cannot be undone.')) {
        localStorage.removeItem('DataTables_stateSavingTable_/');
        location.reload();
    }
});
```

---

## 🔍 Debug: Xem State Được Lưu

### 1. Dùng Browser DevTools

**Cách mở:**
1. Mở DevTools (`F12`)
2. Vào tab **Application** (hoặc **Storage** trong Firefox)
3. Chọn **Local Storage**
4. Tìm domain của ứng dụng
5. Tìm key bắt đầu với `DataTables_`

**Key format:**
```
DataTables_<tableId>_<pathname>
```

**Ví dụ:**
```
DataTables_stateSavingTable_/Home/StateSavingDemo
```

### 2. Log State Trong Console

```javascript
var state = table.state();
console.log(JSON.stringify(state, null, 2));
```

**Output:**
```json
{
  "time": 1705325445123,
  "start": 0,
  "length": 10,
  "order": [[2, "asc"]],
  "search": {
    "search": "^Fruit$",
    "regex": true,
    "smart": true,
    "caseInsensitive": true
  },
  "columns": [
    {
      "visible": true,
      "search": { "search": "", "regex": false, ... }
    },
    ...
  ]
}
```

### 3. Tạo Display Panel

```javascript
function updateStateInfo(api) {
    var state = api.state();
    if (state) {
        var info = {
            'Current Page': Math.floor(state.start / state.length) + 1,
            'Page Length': state.length,
            'Search': state.search.search || '(none)',
            'Sort Column': state.order.length > 0 ? 'Col ' + state.order[0][0] : '(none)',
            'Sort Dir': state.order.length > 0 ? state.order[0][1].toUpperCase() : '(none)',
            'Saved At': new Date(state.time).toLocaleString()
        };

        console.table(info);
        $('#stateInfo').text(JSON.stringify(info, null, 2));
    }
}

// Gọi mỗi khi state thay đổi
table.on('stateLoaded.dt stateSaved.dt', function () {
    updateStateInfo(table);
});
```

---

## ✅ Best Practices

### 1️⃣ Chọn Duration Phù Hợp

```javascript
// 📌 E-commerce site: Permanent
$('#shopTable').DataTable({
    stateSave: true
    // Người dùng muốn nhớ filter sản phẩm
});

// 📌 Analytics dashboard: Session-only
$('#analyticsTable').DataTable({
    stateSave: true,
    stateDuration: -1
    // Data thường thay đổi, không cần nhớ lâu
});

// 📌 Admin panel: 24 hours
$('#adminTable').DataTable({
    stateSave: true,
    stateDuration: 60 * 60 * 24
    // Cân bằng giữa tiện dụng và tính mới của dữ liệu
});
```

### 2️⃣ Sync State Across Tabs

```javascript
// Lắng nghe storage events từ tab khác
window.addEventListener('storage', function (e) {
    if (e.key && e.key.startsWith('DataTables_')) {
        console.log('State changed in another tab, reloading...');
        location.reload();
    }
});
```

### 3️⃣ Provide User Control

```html
<!-- Giúp user biết state được lưu -->
<div class="alert alert-info">
    <small>📌 Table state is automatically saved. 
    <a href="#" onclick="clearState()">Clear saved state</a></small>
</div>
```

### 4️⃣ Performance Considerations

```javascript
// Nếu table có rất nhiều cột, state file sẽ lớn
// Có thể set selective state saving

$.fn.DataTable.ext.feature.push({
    fnInit: function (settings) {
        // Custom logic để optimize state
    }
});
```

---

## 🚨 Common Issues & Solutions

### ❌ Issue 1: State Không Được Lưu

**Nguyên nhân:** localStorage bị disable hoặc full

```javascript
// ✅ Solution: Check localStorage availability
function isStorageAvailable() {
    try {
        var test = '__storage_test__';
        localStorage.setItem(test, test);
        localStorage.removeItem(test);
        return true;
    } catch (e) {
        return false;
    }
}

if (isStorageAvailable()) {
    table = $('#myTable').DataTable({ stateSave: true });
} else {
    console.warn('localStorage not available');
}
```

### ❌ Issue 2: Filter UI Không Khôi Phục

**Nguyên nhân:** Custom filters không kết hợp với state loading

```javascript
// ✅ Solution: Luôn restore UI trong initComplete
initComplete: function () {
    var state = this.api().state.loaded();
    // ... restore logic ...
}
```

### ❌ Issue 3: State Lỗi Sau Update

**Nguyên nhân:** Cấu trúc table thay đổi nhưng state cũ vẫn lưu

```javascript
// ✅ Solution: Force clear state khi version thay đổi
var DATATABLE_VERSION = '1.2';
var stateKey = 'DataTables_' + DATATABLE_VERSION + '_table';

// Or use stateLoadCallback để validate
$('#myTable').DataTable({
    stateSave: true,
    stateLoadCallback: function (settings, callback) {
        // Validate state trước khi apply
        var state = localStorage.getItem('...');
        if (isStateValid(state)) {
            callback(JSON.parse(state));
        } else {
            callback(null);  // Bỏ qua state không hợp lệ
        }
    }
});
```

---

## 📖 API Reference

### Methods

```javascript
// Get current state
var state = table.state();

// Load previously saved state
var savedState = table.state.loaded();

// Clear state (trigger new state save)
table.state.clear();

// Save state manually
table.state.save();
```

### Options

```javascript
DataTable({
    stateSave: true|false,              // Enable/disable state saving
    stateDuration: seconds,             // Duration (-1 = session, 0+ = expire time)
    stateLoadCallback: function() {},   // Callback when loading state
    stateSaveCallback: function() {},   // Callback when saving state
})
```

### Events

```javascript
// Fired when state is loaded
table.on('stateLoaded.dt', function() {});

// Fired when state is saved
table.on('stateSaved.dt', function() {});
```

---

## 🎓 Tổng Kết

| Aspect | Giải Pháp |
|--------|----------|
| **Enable** | `stateSave: true` |
| **Duration** | `stateDuration: seconds` |
| **Session-only** | `stateDuration: -1` |
| **Restore UI** | Check `state.loaded()` in `initComplete` |
| **Debug** | DevTools → Application → Local Storage |
| **Clear** | `localStorage.removeItem(key)` |
| **Listen** | `table.on('stateLoaded.dt', ...)` |

---

**Demo Page:** `/Home/StateSavingDemo`

