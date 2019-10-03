<template>
  <div class="date_parameter">
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

type Time = {
  HH: string,
  mm: string,
  ss: string
};

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

  public created(): void {
    this.stringToDateTime(this.options.value || new Date());
    this.value = this.date.toString();
  }

  private stringToDateTime(value: string): void {
    this.date = new Date(value);

    if (isNaN(this.date.getTime())) {
      this.date = new Date();
      this.time = {
        HH: '0',
        mm: '0',
        ss: '0'
      };

      return;
    }

    const hh = this.date.getHours() + '';
    const mm = this.date.getMinutes() + '';
    const ss = this.date.getSeconds() + '';

    this.time = {
      HH: hh.length > 1 ? hh : `0${hh}`,
      mm: mm.length > 1 ? mm : `0${mm}`,
      ss: ss.length > 1 ? ss : `0${ss}`
    };
  }

  private dateTimeToString(): string {
    return `${this.date.getFullYear()}-${this.date.getMonth() + 1}-${this.date.getDate()} ${this.time.HH}:${this.time.mm}:${this.time.ss}`;
  }

  @Watch('date')
  onChangeValue() {
    this.value = this.dateTimeToString();
    this.$emit('change', this.value);
  }

  @Watch('time')
  onChangeTime() {
    this.value = this.dateTimeToString();
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
