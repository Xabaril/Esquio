<template>
  <div class="date_parameter col-12" data-testid="date-parameter">
    <div class="date_parameter-container">
      <div class="date_parameter-column">
        <date-picker
          v-model="date"
          :format="format.date"
          input-class="date_parameter-input"
          calendar-class="date_parameter-calendar"
        />
      </div>
      <div class="date_parameter-column">
      <vue-timepicker
        v-model="time"
        :format="format.time"
        :second-interval="15"
        class="date_parameter-time"
      />
      </div>
    </div>

  </div>
</template>

<script lang="ts">
import { Component, Vue, Prop, Watch } from 'vue-property-decorator';
import DatePicker from 'vuejs-datepicker';
import VueTimepicker from 'vue2-timepicker';
import { Time, IDateService } from '~/shared';
import { Inject } from 'inversify-props';

@Component({
  components: {
    DatePicker,
    VueTimepicker
  }
})
export default class extends Vue {
  public name = 'DateParameter';
  public value = null;
  public date: Date = null;
  public time: Time  = null;
  public format = {
    date: 'yyyy-MM-dd',
    time: 'HH:mm:ss'
  };

  @Prop({ required: true }) options: any;
  @Inject() dateService: IDateService;

  public created(): void {
    this.stringToDateTime(this.options.value || new Date());
    this.value = this.date.toString();
  }

  private stringToDateTime(value: string): void {
    const [date, time] = this.dateService.stringToDateTime(value);
    this.date = date;
    this.time = time;
  }

  @Watch('date')
  onChangeValue() {
    this.value = this.dateService.dateTimeToString(this.date, this.time);
    this.$emit('change', this.value);
  }

  @Watch('time')
  onChangeTime() {
    this.value = this.dateService.dateTimeToString(this.date, this.time);
    this.$emit('change', this.value);
  }
}
</script>
<style lang="scss" scoped>
.date_parameter {
  &-container {
    display: block;
  }

  &-column {
    display: inline-block;
  }

  /deep/ &-input,
  &-time /deep/ input {
    $height: 2rem;

    border: 1px solid get-color(basic, brighter);
    font-size: get-font-size(m);
    height: $height;
    line-height: $height;
    padding-left: .75rem !important;
  }

  &-time /deep/ input {
    border-left: 0;
    transform: translateY(-1px);
  }

  /deep/ &-calendar {
    .cell.selected {
      color: get-color(basic, brightest) !important;
      background: get-color(secondary) !important;
    }

    .cell:hover {
      border-color: get-color(secondary) !important;
    }
  }
}
</style>
