import qs from 'qs';

export function addQueryParams(
  url: string,
  params: { [key: string]: string | number | boolean | undefined },
): string {
  const stringParams = qs.stringify(params, {
    addQueryPrefix: true,
    skipNulls: true,
  });

  return url + stringParams;
}
