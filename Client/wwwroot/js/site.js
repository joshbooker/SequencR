function init(val) {
    $(".dial").knob({
        'release' : function (v) { 
            window.bpmFunctions.logBpm(v);
         }
    });
}

function moveToStep(step) {
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