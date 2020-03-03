function init(val) {
    console.log('init');

    $(".dial").knob({
        'release' : function (v) { 
            console.log('BPM set to ' + v);
            window.bpmFunctions.logBpm(v);
         }
    });
}

function log(target) {
    console.log(target);
}

function moveToStep(step) {
    console.log('moveToStep ' + step);
    $('.card-step').removeClass('border-primary');
    $('.card-step').removeClass('bg-secondary');
    var stepCard = $('.card-step[data-step="' + step + '"]');
    stepCard.addClass('border-primary');
    stepCard.addClass('bg-secondary');
}

window.bpmFunctions = {
    bpm: 120,
    dotnet: null,
    wireUp: function (dotnetHelper) {
        this.dotnet = dotnetHelper;
    },
    logBpm: function (bpm) {
        return this.dotnet
            .invokeMethodAsync('LogBpm', bpm)
                .then(r => console.log(r));
    }
};