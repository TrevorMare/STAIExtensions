interface Number {
    round(decimalPlaces: number): number;
}
Number.prototype.round = function round(decimalPlaces: number = 0) {
    var p = Math.pow(10, decimalPlaces);
    var n = (parseFloat(this.toString()) * p) * (1 + Number.EPSILON);
    return Math.round(n) / p;
}

