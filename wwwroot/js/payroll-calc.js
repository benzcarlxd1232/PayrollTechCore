// Live payroll computation preview
(function () {
    const empSelect = document.getElementById('employeeSelect');
    const daysInput = document.getElementById('daysWorked');
    const deductionInput = document.getElementById('deduction');
    const grossDisplay = document.getElementById('grossPayDisplay');
    const deductionDisplay = document.getElementById('deductionDisplay');
    const netDisplay = document.getElementById('netPayDisplay');

    function fmt(val) {
        return '₱' + parseFloat(val || 0).toLocaleString('en-PH', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
    }

    function recalc() {
        const empId = empSelect ? empSelect.value : null;
        const days = parseFloat(daysInput ? daysInput.value : 0) || 0;
        const deduction = parseFloat(deductionInput ? deductionInput.value : 0) || 0;

        // Try to get daily rate from the employee name text (not available client-side easily)
        // We rely on server-side computation; this is just a preview hint
        grossDisplay.textContent = '(computed on save)';
        deductionDisplay.textContent = fmt(deduction);
        netDisplay.textContent = '(computed on save)';
    }

    if (empSelect) empSelect.addEventListener('change', recalc);
    if (daysInput) daysInput.addEventListener('input', recalc);
    if (deductionInput) deductionInput.addEventListener('input', function () {
        deductionDisplay.textContent = fmt(this.value);
    });
})();
