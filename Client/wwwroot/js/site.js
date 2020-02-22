function init(val) {
    console.log('init');

    $(".dial").knob({
        'release' : function (v) { 
            console.log('BPM set to ' + v);
         }
    });
}