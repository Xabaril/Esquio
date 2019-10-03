import { render, fireEvent, cleanup } from '@testing-library/vue';
import { ParameterDetailType } from '.';
import ParameterComponent from './Parameter.vue';
import { createLocalVue } from '@vue/test-utils';
import VeeValidate from 'vee-validate';

describe('Parameter Component', () => {
  let localVue = createLocalVue();
  localVue.use(VeeValidate);
  afterEach(cleanup);

  it('When type is StringParameter should render the StringParameter component', () => {
    const type = ParameterDetailType.EsquioString;
    const options = {};

    const { getByTestId } = render(ParameterComponent, {
      localVue,
      mocks: { $t: x => x },
      props: {type, options}
    });
    const element = getByTestId('string-parameter');

    expect(element).toBeTruthy();
  });

  it('When type is SemicolonParameter should render the SemicolonParameter component', () => {
    const type = ParameterDetailType.EsquioSemicolonList;
    const options = {};

    const { getByTestId } = render(ParameterComponent, {
      localVue,
      mocks: { $t: x => x },
      props: {type, options}
    });
    const element = getByTestId('semicolon-parameter');

    expect(element).toBeTruthy();
  });

  it('When type is PercentageParameter should render the PercentageParameter component', () => {
    const type = ParameterDetailType.EsquioPercentage;
    const options = { value: 50 };

    const { getByTestId } = render(ParameterComponent, {
      localVue,
      mocks: { $t: x => x },
      props: {type, options}
    });
    const element = getByTestId('percentage-parameter');

    expect(element).toBeTruthy();
  });

  it('When type is DateParameter should render the DateParameter component', () => {
    const type = ParameterDetailType.EsquioDate;
    const options = {};

    const { getByTestId } = render(ParameterComponent, {
      localVue,
      mocks: { $t: x => x },
      props: {type, options}
    });
    const element = getByTestId('date-parameter');

    expect(element).toBeTruthy();
  });
});
