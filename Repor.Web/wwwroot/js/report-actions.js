document.addEventListener("DOMContentLoaded", function () {

    // مشاهده گزارش
    document.querySelectorAll(".view-report").forEach(btn => {
        btn.addEventListener("click", function () {
            const id = this.dataset.id;
            window.location.href = `/ReportManagement/Details/${id}`;
        });
    });

    // تایید گزارش
    document.querySelectorAll(".approve-report").forEach(btn => {
        btn.addEventListener("click", function () {
            const id = this.dataset.id;

            Swal.fire({
                title: 'تایید گزارش',
                text: 'آیا از تایید این گزارش مطمئن هستید؟',
                icon: 'question',
                showCancelButton: true,
                confirmButtonText: 'بله',
                cancelButtonText: 'لغو'
            }).then(result => {
                if (result.isConfirmed) {
                    window.location.href = `/ReportManagement/Approve/${id}`;
                }
            });
        });
    });

    // رد گزارش
    document.querySelectorAll(".reject-report").forEach(btn => {
        btn.addEventListener("click", function () {
            const id = this.dataset.id;

            Swal.fire({
                title: 'رد گزارش',
                text: 'آیا می‌خواهید این گزارش رد شود؟',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'رد شود',
                cancelButtonText: 'لغو'
            }).then(result => {
                if (result.isConfirmed) {
                    window.location.href = `/ReportManagement/Reject/${id}`;
                }
            });
        });
    });

    // حذف گزارش
    document.querySelectorAll(".delete-report").forEach(btn => {
        btn.addEventListener("click", function () {
            const id = this.dataset.id;

            Swal.fire({
                title: 'حذف گزارش',
                text: 'این عملیات قابل بازگشت نیست!',
                icon: 'error',
                showCancelButton: true,
                confirmButtonText: 'حذف',
                cancelButtonText: 'لغو'
            }).then(result => {
                if (result.isConfirmed) {
                    window.location.href = `/ReportManagement/Delete/${id}`;
                }
            });
        });
    });

});


