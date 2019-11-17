import { injectable } from 'inversify-props';
import { settings } from '~/core';
import { FormTag } from './form-tag.model';
import { ITagsService } from './itags.service';
import { Tag } from './tag.model';

@injectable()
export class TagsService implements ITagsService {
  public async get(productName: string, flagName: string): Promise<Tag[]> {
    const response = await fetch(`${settings.ApiUrl}/products/${productName}/features/${flagName}/tags`);

    if (!response.ok) {
      throw new Error('Cannot fetch tags');
    }

    return response.json();
  }

  public async add(productName: string, flagName: string, tag: Tag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/${productName}/features/${flagName}/tags/tag`, {
      method: 'POST',
      body: JSON.stringify({
        tag: tag.name
      })
    });

    if (!response.ok) {
      throw new Error(`Cannot create tag ${tag.name}`);
    }
  }

  public async remove(productName: string, flagName: string, tag: Tag): Promise<void> {
    const response = await fetch(`${settings.ApiUrl}/products/${productName}/features/${flagName}/tags/untag/${tag.name}`, {
      method: 'DELETE'
    });

    if (!response.ok) {
      throw new Error(`Cannot delete flag ${tag.name}`);
    }
  }

  public toFormTags(tags: Tag[]): FormTag[] {
    return tags.map(({name}) => {
      return {
        text: name
      } as FormTag;
    });
  }

  public toTags(formTags: FormTag[]): Tag[] {
    return formTags.map(({text}) => {
      return {
        name: text
      } as Tag;
    });
  }
}

