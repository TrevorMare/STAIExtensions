// Got this from the following code pen and made some adjustments to make it work
// https://codepen.io/arturgolenia/pen/qRmoZW


function animateCircle() {
    function rotateCircles() {
        $('.arc-svg-path').each(function() {
            var that = this,
            direction = ["-", "+"],
            chosenDirection = direction[Math.floor(Math.random() * direction.length)],
            speed = Math.floor((Math.random() * 250) + 100),
            looper = setInterval(circleMove, 2000);
        
            function circleMove() {
            $(that).animate({
                rotation: chosenDirection + '=' + speed
            }, {
                duration: 2000,
                easing: 'linear',
                step: function(now) {
                $(that).css({
                    "transform": "rotate(" + now + "deg)"
                });
                }
            });
            }
            circleMove();
        });
    }
    rotateCircles();
} 


 