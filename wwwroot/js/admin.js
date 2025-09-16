// Admin Panel JavaScript Functions

// Update Order Status
function updateOrderStatus(orderId, status) {
    if (!confirm('Bạn có chắc chắn muốn cập nhật trạng thái đơn hàng này?')) {
        return;
    }

    const button = event.target;
    const originalText = button.innerHTML;
    button.innerHTML = '<i class="fas fa-spinner fa-spin"></i>';
    button.disabled = true;

    fetch('/Admin/UpdateOrderStatus', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: `orderId=${orderId}&status=${status}`
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            showToast(data.message, 'success');
            location.reload();
        } else {
            showToast(data.message || 'Có lỗi xảy ra!', 'error');
        }
    })
    .catch(error => {
        showToast('Có lỗi xảy ra khi cập nhật!', 'error');
    })
    .finally(() => {
        button.innerHTML = originalText;
        button.disabled = false;
    });
}

// Toggle Product Status
function toggleProductStatus(productId) {
    if (!confirm('Bạn có chắc chắn muốn thay đổi trạng thái sản phẩm này?')) {
        return;
    }

    const button = event.target;
    const originalText = button.innerHTML;
    button.innerHTML = '<i class="fas fa-spinner fa-spin"></i>';
    button.disabled = true;

    fetch('/Admin/ToggleProductStatus', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
        },
        body: `productId=${productId}`
    })
    .then(response => response.json())
    .then(data => {
        if (data.success) {
            showToast(data.message, 'success');
            location.reload();
        } else {
            showToast(data.message || 'Có lỗi xảy ra!', 'error');
        }
    })
    .catch(error => {
        showToast('Có lỗi xảy ra khi cập nhật!', 'error');
    })
    .finally(() => {
        button.innerHTML = originalText;
        button.disabled = false;
    });
}

// Format Currency for Admin
function formatCurrencyAdmin(amount) {
    return new Intl.NumberFormat('vi-VN', {
        style: 'currency',
        currency: 'VND'
    }).format(amount);
}

// Initialize Admin Dashboard
document.addEventListener('DOMContentLoaded', function() {
    // Initialize tooltips
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Initialize popovers
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'));
    var popoverList = popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl);
    });

    // Auto-refresh dashboard every 5 minutes
    if (window.location.pathname.includes('/Admin') && window.location.pathname.endsWith('/Admin')) {
        setInterval(function() {
            // Only refresh if user is still on the page
            if (document.visibilityState === 'visible') {
                location.reload();
            }
        }, 300000); // 5 minutes
    }

    // Add loading states to forms
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function() {
            const submitBtn = form.querySelector('button[type="submit"]');
            if (submitBtn) {
                submitBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Đang xử lý...';
                submitBtn.disabled = true;
            }
        });
    });

    // Sidebar toggle for mobile
    const sidebarToggle = document.getElementById('sidebarToggle');
    const sidebar = document.querySelector('.sidebar');
    
    if (sidebarToggle && sidebar) {
        sidebarToggle.addEventListener('click', function() {
            sidebar.classList.toggle('show');
        });
    }

    // Close sidebar when clicking outside on mobile
    document.addEventListener('click', function(event) {
        if (window.innerWidth <= 768) {
            const sidebar = document.querySelector('.sidebar');
            const sidebarToggle = document.getElementById('sidebarToggle');
            
            if (sidebar && !sidebar.contains(event.target) && 
                sidebarToggle && !sidebarToggle.contains(event.target)) {
                sidebar.classList.remove('show');
            }
        }
    });
});

// Confirm Delete Actions
function confirmDelete(message = 'Bạn có chắc chắn muốn xóa?') {
    return confirm(message);
}

// Export Data Functions
function exportToCSV(tableId, filename = 'export.csv') {
    const table = document.getElementById(tableId);
    if (!table) return;

    let csv = [];
    const rows = table.querySelectorAll('tr');

    for (let i = 0; i < rows.length; i++) {
        const row = [];
        const cols = rows[i].querySelectorAll('td, th');

        for (let j = 0; j < cols.length; j++) {
            let text = cols[j].innerText;
            text = text.replace(/"/g, '""'); // Escape quotes
            row.push('"' + text + '"');
        }

        csv.push(row.join(','));
    }

    // Download CSV
    const csvFile = new Blob([csv.join('\n')], { type: 'text/csv' });
    const downloadLink = document.createElement('a');
    downloadLink.download = filename;
    downloadLink.href = window.URL.createObjectURL(csvFile);
    downloadLink.style.display = 'none';
    document.body.appendChild(downloadLink);
    downloadLink.click();
    document.body.removeChild(downloadLink);
}

// Print Functions
function printTable(tableId) {
    const table = document.getElementById(tableId);
    if (!table) return;

    const printWindow = window.open('', '', 'height=600,width=800');
    printWindow.document.write('<html><head><title>Print</title>');
    printWindow.document.write('<link rel="stylesheet" href="/lib/bootstrap/dist/css/bootstrap.min.css">');
    printWindow.document.write('</head><body>');
    printWindow.document.write(table.outerHTML);
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    printWindow.print();
}

// Search and Filter Functions
function filterTable(inputId, tableId) {
    const input = document.getElementById(inputId);
    const table = document.getElementById(tableId);
    
    if (!input || !table) return;

    input.addEventListener('keyup', function() {
        const filter = input.value.toLowerCase();
        const rows = table.getElementsByTagName('tr');

        for (let i = 1; i < rows.length; i++) { // Skip header row
            const row = rows[i];
            const cells = row.getElementsByTagName('td');
            let found = false;

            for (let j = 0; j < cells.length; j++) {
                if (cells[j].innerText.toLowerCase().indexOf(filter) > -1) {
                    found = true;
                    break;
                }
            }

            row.style.display = found ? '' : 'none';
        }
    });
}

// Initialize search functionality
document.addEventListener('DOMContentLoaded', function() {
    // Auto-initialize search for tables with search inputs
    const searchInputs = document.querySelectorAll('[data-table-search]');
    searchInputs.forEach(input => {
        const tableId = input.getAttribute('data-table-search');
        filterTable(input.id, tableId);
    });
});
